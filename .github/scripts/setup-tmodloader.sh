#!/usr/bin/env bash
set -euo pipefail

: "${TML_VERSION:?TML_VERSION must be set}"
: "${TML_DIR:?TML_DIR must be set}"
: "${RUNNER_TEMP:?RUNNER_TEMP must be set}"

archive="${RUNNER_TEMP}/tModLoader-${TML_VERSION}.zip"
if [[ ! -f "${archive}" ]]; then
  curl --fail --location --retry 3 \
    --output "${archive}" \
    "https://github.com/tModLoader/tModLoader/releases/download/${TML_VERSION}/tModLoader.zip"
else
  echo "Using cached tModLoader archive at ${archive}."
fi

unzip -q "${archive}" -d "${TML_DIR}"

chmod +x "${TML_DIR}"/*.sh || true

cat > "${TML_DIR}/ModSources/tModLoader.targets" <<'EOF'
<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(MSBuildThisFileDirectory)../tMLMod.targets" />
</Project>
EOF

sdk_version="$(dotnet --list-sdks | awk '/^8\.0\./ { print $1 }' | sort -V | tail -n 1)"
if [[ -z "${sdk_version}" ]]; then
  echo "A .NET 8 SDK was not installed." >&2
  exit 1
fi

dotnet new globaljson \
  --sdk-version "${sdk_version}" \
  --roll-forward latestPatch \
  --force \
  --output "${TML_DIR}"

ln -sf \
  "${TML_DIR}/Libraries/Native/Linux/libSDL2-2.0.so.0" \
  "${TML_DIR}/Libraries/Native/Linux/libSDL2.so"
