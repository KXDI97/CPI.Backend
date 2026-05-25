#!/usr/bin/env bash
# ─────────────────────────────────────────────────────────────────────────────
# stop-all.sh — Detiene todos los microservicios CPI que estén corriendo
# Uso: ./stop-all.sh
# ─────────────────────────────────────────────────────────────────────────────

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PID_FILE="$SCRIPT_DIR/.cpi-pids"

RESET="\033[0m"
BOLD="\033[1m"
C_OK="\033[1;32m"
C_WARN="\033[1;33m"
C_ERR="\033[1;31m"
C_INFO="\033[0;37m"

echo ""
echo -e "${BOLD}╔══════════════════════════════════════════════╗${RESET}"
echo -e "${BOLD}║       CPI — Deteniendo microservicios        ║${RESET}"
echo -e "${BOLD}╚══════════════════════════════════════════════╝${RESET}"
echo ""

STOPPED=0
NOT_FOUND=0

# ── Por PID file (sesión de start-all.sh) ────────────────────────────────────
if [[ -f "$PID_FILE" ]]; then
  while IFS= read -r pid; do
    [[ -z "$pid" ]] && continue
    if kill -0 "$pid" 2>/dev/null; then
      kill "$pid" 2>/dev/null
      echo -e "${C_OK}[OK]${RESET}   PID $pid detenido."
      ((STOPPED++)) || true
    else
      echo -e "${C_WARN}[SKIP]${RESET} PID $pid ya no estaba corriendo."
      ((NOT_FOUND++)) || true
    fi
  done < "$PID_FILE"
  rm -f "$PID_FILE"
else
  echo -e "${C_WARN}[WARN]${RESET} No se encontró $PID_FILE — buscando por nombre de proceso..."
fi

# ── Por nombre de proceso (fallback si no hay PID file) ──────────────────────
declare -a PATTERNS=(
  "AuthService.Api"
  "CatalogService.Api"
  "InventoryService.Api"
  "SalesService.Api"
)

for pattern in "${PATTERNS[@]}"; do
  pids=$(pgrep -f "dotnet.*${pattern}" 2>/dev/null || true)
  if [[ -n "$pids" ]]; then
    echo "$pids" | while read -r pid; do
      kill "$pid" 2>/dev/null && echo -e "${C_OK}[OK]${RESET}   $pattern (PID $pid) detenido."
      ((STOPPED++)) || true
    done
  fi
done

echo ""
if [[ $STOPPED -gt 0 ]]; then
  echo -e "${C_OK}${BOLD}Listo.${RESET} $STOPPED proceso(s) detenido(s)."
else
  echo -e "${C_INFO}No había servicios CPI corriendo.${RESET}"
fi
echo ""
