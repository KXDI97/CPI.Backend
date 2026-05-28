#!/usr/bin/env bash
# ─────────────────────────────────────────────────────────────────────────────
# start-all.sh — Levanta todos los microservicios CPI en paralelo
# Uso: ./start-all.sh
# ─────────────────────────────────────────────────────────────────────────────

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PID_FILE="$SCRIPT_DIR/.cpi-pids"
LOG_DIR="$SCRIPT_DIR/.cpi-logs"

mkdir -p "$LOG_DIR"

# ── Colores ANSI ────────────────────────────────────────────────────────────
RESET="\033[0m"
BOLD="\033[1m"
C_AUTH="\033[1;36m"       # cyan
C_CATALOG="\033[1;33m"    # amarillo
C_INVENTORY="\033[1;32m"  # verde
C_SALES="\033[1;35m"      # magenta
C_PURCHASE="\033[1;34m"   # azul
C_ERROR="\033[1;31m"      # rojo
C_INFO="\033[0;37m"       # gris

# ── Limpiar instancias previas antes de arrancar ─────────────────────────────
_kill_port() {
  local port=$1
  local pids
  pids=$(lsof -ti :"$port" 2>/dev/null || true)

  if [[ -n "$pids" ]]; then
    echo -e "${C_INFO}[CPI]${RESET} Puerto $port ocupado — liberando..."
    echo "$pids" | xargs kill -9 2>/dev/null || true
  fi
}

pkill -f "dotnet.*AuthService.Api"       2>/dev/null || true
pkill -f "dotnet.*CatalogService.Api"    2>/dev/null || true
pkill -f "dotnet.*InventoryService.Api"  2>/dev/null || true
pkill -f "dotnet.*SalesService.Api"      2>/dev/null || true
pkill -f "dotnet.*PurchaseService.Api"   2>/dev/null || true

for _p in 5258 5131 5035 5215 5062; do
  _kill_port "$_p"
done

sleep 1

> "$PID_FILE"

log_info()  { echo -e "${C_INFO}[CPI]${RESET} $*"; }
log_error() { echo -e "${C_ERROR}[ERROR]${RESET} $*" >&2; }

# ── Definición de servicios ──────────────────────────────────────────────────
# Formato:
# "Nombre|DirectorioRelativo|Puerto|Color"

declare -a SERVICES=(
  "AuthService|AuthService.Api|5258|${C_AUTH}"
  "CatalogService|CatalogService.Api|5131|${C_CATALOG}"
  "InventoryService|InventoryService.Api|5035|${C_INVENTORY}"
  "SalesService|SalesService.Api|5215|${C_SALES}"
  "PurchaseService|PurchaseService.Api|5062|${C_PURCHASE}"
)

# ── Prefix logs ──────────────────────────────────────────────────────────────
prefix_log() {
  local label="$1"
  local color="$2"

  while IFS= read -r line; do
    echo -e "${color}[${label}]${RESET} ${line}"
  done
}

# ── Iniciar servicio ─────────────────────────────────────────────────────────
start_service() {
  local name="$1"
  local dir="$2"
  local port="$3"
  local color="$4"

  local full_path="$SCRIPT_DIR/$dir"
  local log_file="$LOG_DIR/${name// /}.log"

  if [[ ! -d "$full_path" ]]; then
    log_error "Directorio no encontrado: $full_path"
    return 1
  fi

  echo -e "${color}${BOLD}[${name}]${RESET} Iniciando en http://localhost:${port} ..."

  (
    cd "$full_path"

    dotnet run --launch-profile http 2>&1 \
      | tee "$log_file" \
      | prefix_log "$name" "$color"
  ) &

  local pid=$!

  echo "$pid" >> "$PID_FILE"

  echo -e "${color}[${name}]${RESET} PID: ${BOLD}${pid}${RESET}"
}

# ── Cleanup ──────────────────────────────────────────────────────────────────
cleanup() {
  echo ""

  log_info "Señal recibida. Deteniendo todos los servicios..."

  if [[ -f "$PID_FILE" ]]; then
    while IFS= read -r pid; do
      if kill -0 "$pid" 2>/dev/null; then
        kill "$pid" 2>/dev/null \
          && echo -e "${C_ERROR}[CPI]${RESET} PID $pid detenido."
      fi
    done < "$PID_FILE"

    pkill -f "dotnet.*AuthService.Api"       2>/dev/null || true
    pkill -f "dotnet.*CatalogService.Api"    2>/dev/null || true
    pkill -f "dotnet.*InventoryService.Api"  2>/dev/null || true
    pkill -f "dotnet.*SalesService.Api"      2>/dev/null || true
    pkill -f "dotnet.*PurchaseService.Api"   2>/dev/null || true

    rm -f "$PID_FILE"
  fi

  log_info "Todos los servicios detenidos."

  exit 0
}

trap cleanup SIGINT SIGTERM

# ── Verificar dotnet ─────────────────────────────────────────────────────────
if ! command -v dotnet &>/dev/null; then
  log_error "dotnet no encontrado. Instálalo desde https://dotnet.microsoft.com"
  exit 1
fi

echo ""
echo -e "${BOLD}╔══════════════════════════════════════════════╗${RESET}"
echo -e "${BOLD}║        CPI — Iniciando microservicios       ║${RESET}"
echo -e "${BOLD}╚══════════════════════════════════════════════╝${RESET}"
echo ""

# ── Levantar servicios ───────────────────────────────────────────────────────
for entry in "${SERVICES[@]}"; do
  IFS='|' read -r name dir port color <<< "$entry"

  start_service "$name" "$dir" "$port" "$color"

  sleep 0.5
done

echo ""
echo -e "${C_INFO}Logs: ${BOLD}${LOG_DIR}/${RESET}"
echo -e "${C_INFO}PIDs: ${BOLD}${PID_FILE}${RESET}"
echo -e "${C_INFO}Detener: ${BOLD}Ctrl+C${RESET}"
echo ""

# ── Esperar procesos ─────────────────────────────────────────────────────────
wait