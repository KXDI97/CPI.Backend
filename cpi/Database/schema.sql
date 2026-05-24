/* ============================================================
   Proyecto CPI – Esquema de Base de Datos (PostgreSQL)
   Versión: 2.0 – Convertido desde SQL Server
   ============================================================ */

-- ============================================================
-- Limpieza segura (DROP en orden inverso de dependencias)
-- ============================================================

-- Triggers
DROP TRIGGER IF EXISTS tr_PurchaseReceiptDetails_MoveStock ON "PurchaseReceiptDetails";
DROP TRIGGER IF EXISTS tr_SalesDetails_MoveStock           ON "SalesDetails";

-- Funciones de triggers
DROP FUNCTION IF EXISTS fn_PurchaseReceiptDetails_MoveStock();
DROP FUNCTION IF EXISTS fn_SalesDetails_MoveStock();

-- Procedimientos
DROP PROCEDURE IF EXISTS sp_Sales_RecalculateTotals(INT, DECIMAL);

-- Vistas
DROP VIEW IF EXISTS v_CurrentStock;
DROP VIEW IF EXISTS v_SalesLines;
DROP VIEW IF EXISTS v_SalesSummary;

-- Tablas (orden inverso de FK)
DROP TABLE IF EXISTS "InventoryMovements"     CASCADE;
DROP TABLE IF EXISTS "PurchaseReceiptDetails" CASCADE;
DROP TABLE IF EXISTS "PurchaseReceipts"       CASCADE;
DROP TABLE IF EXISTS "SalesDetails"           CASCADE;
DROP TABLE IF EXISTS "Sales"                  CASCADE;
DROP TABLE IF EXISTS "Transactions"           CASCADE;
DROP TABLE IF EXISTS "LogicalCosts"           CASCADE;
DROP TABLE IF EXISTS "PurchaseOrderDetails"   CASCADE;
DROP TABLE IF EXISTS "PurchaseOrders"         CASCADE;
DROP TABLE IF EXISTS "Products"               CASCADE;
DROP TABLE IF EXISTS "Clients"                CASCADE;
DROP TABLE IF EXISTS "Suppliers"              CASCADE;
DROP TABLE IF EXISTS "Users"                  CASCADE;

-- ============================================================
-- Users
-- ============================================================
CREATE TABLE "Users" (
    "ID"           SERIAL                   PRIMARY KEY,
    "Username"     VARCHAR(50)              NOT NULL,
    "Email"        VARCHAR(100)             NOT NULL,
    "Role"         VARCHAR(20)              NOT NULL,
    "PasswordHash" BYTEA                    NULL,
    "PasswordSalt" BYTEA                    NULL,
    "CreatedAt"    TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "UQ_Users_Username" UNIQUE ("Username"),
    CONSTRAINT "UQ_Users_Email"    UNIQUE ("Email"),
    CONSTRAINT "CK_Users_Role"     CHECK  ("Role" IN ('Admin', 'Seller', 'Viewer', 'Deactivated'))
);

-- ============================================================
-- Suppliers
-- ============================================================
CREATE TABLE "Suppliers" (
    "SupplierId" SERIAL       PRIMARY KEY,
    "Name"       VARCHAR(100) NOT NULL,
    "Contact"    VARCHAR(100) NULL,
    "Phone"      VARCHAR(20)  NULL,
    "Email"      VARCHAR(100) NULL,
    "Address"    VARCHAR(255) NULL
);

-- ============================================================
-- Clients
-- ============================================================
CREATE TABLE "Clients" (
    "ClientId"     SERIAL       PRIMARY KEY,
    "Name"         VARCHAR(150) NOT NULL,
    "ClientType"   VARCHAR(20)  NOT NULL DEFAULT 'Empresa',
    "DocumentType" VARCHAR(20)  NOT NULL DEFAULT 'RUT',
    "DocumentID"   VARCHAR(40)  NOT NULL,
    "Email"        VARCHAR(100) NULL,
    "Phone"        VARCHAR(30)  NULL,
    "Website"      VARCHAR(150) NULL,
    "Address"      VARCHAR(255) NULL,
    CONSTRAINT "UQ_Clients_Doc" UNIQUE ("DocumentType", "DocumentID")
);

-- ============================================================
-- Products
-- ============================================================
CREATE TABLE "Products" (
    "ProductId"   VARCHAR(50)   NOT NULL PRIMARY KEY,
    "Name"        VARCHAR(100)  NOT NULL,
    "Value"       DECIMAL(19,4) NOT NULL,
    "Category"    VARCHAR(50)   NOT NULL,
    "Description" VARCHAR(255)  NOT NULL,
    "Stock"       DECIMAL(18,4) NOT NULL DEFAULT 0,
    "SupplierId"  INT           NOT NULL,
    CONSTRAINT "FK_Products_Suppliers" FOREIGN KEY ("SupplierId")
        REFERENCES "Suppliers"("SupplierId")
);

-- ============================================================
-- PurchaseOrders
-- ============================================================
CREATE TABLE "PurchaseOrders" (
    "PurchaseOrderId" SERIAL                   PRIMARY KEY,
    "SupplierId"      INT                      NOT NULL,
    "OrderDate"       TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "Status"          VARCHAR(20)              NOT NULL,
    CONSTRAINT "FK_PurchaseOrders_Suppliers" FOREIGN KEY ("SupplierId")
        REFERENCES "Suppliers"("SupplierId")
);

-- ============================================================
-- PurchaseOrderDetails
-- ============================================================
CREATE TABLE "PurchaseOrderDetails" (
    "PurchaseOrderDetailId" SERIAL        PRIMARY KEY,
    "PurchaseOrderId"       INT           NOT NULL,
    "ProductId"             VARCHAR(50)   NOT NULL,
    "Quantity"              DECIMAL(18,4) NOT NULL,
    "UnitPrice"             DECIMAL(19,4) NOT NULL,
    "LineTotal"             DECIMAL(19,4) GENERATED ALWAYS AS (CAST("Quantity" AS DECIMAL(19,4)) * "UnitPrice") STORED,
    CONSTRAINT "FK_POD_PO"    FOREIGN KEY ("PurchaseOrderId") REFERENCES "PurchaseOrders"("PurchaseOrderId"),
    CONSTRAINT "FK_POD_P"     FOREIGN KEY ("ProductId")       REFERENCES "Products"("ProductId"),
    CONSTRAINT "CK_POD_Qty"   CHECK ("Quantity"  > 0),
    CONSTRAINT "CK_POD_Price" CHECK ("UnitPrice" >= 0)
);

-- ============================================================
-- LogicalCosts
-- ============================================================
CREATE TABLE "LogicalCosts" (
    "Order_Number"            INT           NOT NULL PRIMARY KEY,
    "International_Transport" DECIMAL(19,4) NOT NULL,
    "Local_Transport"         DECIMAL(19,4) NOT NULL,
    "Nationalization"         DECIMAL(19,4) NOT NULL,
    "Cargo_Insurance"         DECIMAL(19,4) NOT NULL,
    "Storage"                 DECIMAL(19,4) NOT NULL,
    "Others"                  DECIMAL(19,4) NOT NULL
);

-- ============================================================
-- Transactions
-- ============================================================
CREATE TABLE "Transactions" (
    "Transaction_Number" SERIAL       PRIMARY KEY,
    "PurchaseOrderId"    INT          NOT NULL,
    "Invoice_Number"     VARCHAR(50)  NOT NULL,
    "Reminder"           VARCHAR(100) NULL,
    "Transaction_Status" VARCHAR(20)  NOT NULL,
    "Payment_Date"       DATE         NULL,
    CONSTRAINT "FK_Transactions_PO" FOREIGN KEY ("PurchaseOrderId")
        REFERENCES "PurchaseOrders"("PurchaseOrderId")
);

-- ============================================================
-- Sales
-- ============================================================
CREATE TABLE "Sales" (
    "InvoiceId"    SERIAL                   PRIMARY KEY,
    "ClientId"     INT                      NOT NULL,
    "InvoiceDate"  TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "Status"       VARCHAR(20)              NOT NULL DEFAULT 'Emitida',
    "ExchangeRate" DECIMAL(18,4)            NULL,
    "Subtotal"     DECIMAL(19,4)            NOT NULL DEFAULT 0,
    "Tax"          DECIMAL(19,4)            NOT NULL DEFAULT 0,
    "Total"        DECIMAL(19,4)            NOT NULL DEFAULT 0,
    CONSTRAINT "FK_Sales_Clients" FOREIGN KEY ("ClientId") REFERENCES "Clients"("ClientId")
);

-- ============================================================
-- SalesDetails
-- ============================================================
CREATE TABLE "SalesDetails" (
    "InvoiceDetailId" SERIAL        PRIMARY KEY,
    "InvoiceId"       INT           NOT NULL,
    "ProductId"       VARCHAR(50)   NOT NULL,
    "Quantity"        DECIMAL(18,4) NOT NULL,
    "UnitPrice"       DECIMAL(19,4) NOT NULL,
    "LineTotal"       DECIMAL(19,4) GENERATED ALWAYS AS (CAST("Quantity" AS DECIMAL(19,4)) * "UnitPrice") STORED,
    CONSTRAINT "FK_SalesDetails_Sales"    FOREIGN KEY ("InvoiceId") REFERENCES "Sales"("InvoiceId"),
    CONSTRAINT "FK_SalesDetails_Products" FOREIGN KEY ("ProductId") REFERENCES "Products"("ProductId"),
    CONSTRAINT "CK_SalesDetails_Qty"      CHECK ("Quantity"  > 0),
    CONSTRAINT "CK_SalesDetails_Price"    CHECK ("UnitPrice" >= 0)
);

-- ============================================================
-- PurchaseReceipts
-- ============================================================
CREATE TABLE "PurchaseReceipts" (
    "ReceiptId"       SERIAL                   PRIMARY KEY,
    "PurchaseOrderId" INT                      NOT NULL,
    "ReceiptDate"     TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_PR_PO" FOREIGN KEY ("PurchaseOrderId") REFERENCES "PurchaseOrders"("PurchaseOrderId")
);

-- ============================================================
-- PurchaseReceiptDetails
-- ============================================================
CREATE TABLE "PurchaseReceiptDetails" (
    "ReceiptDetailId"  SERIAL        PRIMARY KEY,
    "ReceiptId"        INT           NOT NULL,
    "ProductId"        VARCHAR(50)   NOT NULL,
    "QuantityReceived" DECIMAL(18,4) NOT NULL,
    "UnitCost"         DECIMAL(19,4) NOT NULL,
    CONSTRAINT "FK_PRD_R"    FOREIGN KEY ("ReceiptId") REFERENCES "PurchaseReceipts"("ReceiptId"),
    CONSTRAINT "FK_PRD_P"    FOREIGN KEY ("ProductId") REFERENCES "Products"("ProductId"),
    CONSTRAINT "CK_PRD_Qty"  CHECK ("QuantityReceived" > 0),
    CONSTRAINT "CK_PRD_Cost" CHECK ("UnitCost"         >= 0)
);

-- ============================================================
-- InventoryMovements
-- ============================================================
CREATE TABLE "InventoryMovements" (
    "MovementId" SERIAL                   PRIMARY KEY,
    "ProductId"  VARCHAR(50)              NOT NULL,
    "QtyChange"  DECIMAL(18,4)            NOT NULL,
    "SourceType" VARCHAR(20)              NOT NULL,
    "SourceId"   INT                      NULL,
    "CreatedAt"  TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_IM_Products" FOREIGN KEY ("ProductId") REFERENCES "Products"("ProductId")
);

-- ============================================================
-- Índices
-- ============================================================
CREATE INDEX "IX_PurchaseOrders_SupplierId"      ON "PurchaseOrders"("SupplierId");
CREATE INDEX "IX_PurchaseOrderDetails_POId"       ON "PurchaseOrderDetails"("PurchaseOrderId");
CREATE INDEX "IX_PurchaseOrderDetails_ProductId"  ON "PurchaseOrderDetails"("ProductId");
CREATE INDEX "IX_Products_SupplierId"             ON "Products"("SupplierId");
CREATE INDEX "IX_Sales_ClientId"                  ON "Sales"("ClientId");
CREATE INDEX "IX_Sales_InvoiceDate"               ON "Sales"("InvoiceDate");
CREATE INDEX "IX_SalesDetails_InvoiceId"          ON "SalesDetails"("InvoiceId");
CREATE INDEX "IX_SalesDetails_ProductId"          ON "SalesDetails"("ProductId");
CREATE INDEX "IX_Transactions_PO_Status_PDate"    ON "Transactions"("PurchaseOrderId", "Transaction_Status", "Payment_Date");

-- ============================================================
-- Stored Procedure: sp_Sales_RecalculateTotals
-- ============================================================
CREATE OR REPLACE PROCEDURE sp_Sales_RecalculateTotals(
    p_InvoiceId    INT,
    p_ExchangeRate DECIMAL(18,4) DEFAULT NULL
)
LANGUAGE plpgsql AS $$
BEGIN
    WITH totals AS (
        SELECT COALESCE(SUM("LineTotal"), 0) AS SumLineTotal
        FROM "SalesDetails"
        WHERE "InvoiceId" = p_InvoiceId
    )
    UPDATE "Sales"
    SET "Subtotal"     = totals.SumLineTotal,
        "Tax"          = 0,
        "Total"        = totals.SumLineTotal,
        "ExchangeRate" = COALESCE(p_ExchangeRate, "Sales"."ExchangeRate")
    FROM totals
    WHERE "Sales"."InvoiceId" = p_InvoiceId;
END;
$$;

-- ============================================================
-- Trigger: SalesDetails → InventoryMovements → Products.Stock
-- ============================================================
CREATE OR REPLACE FUNCTION fn_SalesDetails_MoveStock()
RETURNS TRIGGER AS $$
DECLARE
    v_product_id VARCHAR(50);
BEGIN
    IF TG_OP = 'INSERT' THEN
        INSERT INTO "InventoryMovements"("ProductId", "QtyChange", "SourceType", "SourceId")
        VALUES (NEW."ProductId", -NEW."Quantity", 'Sale', NEW."InvoiceId");
        v_product_id := NEW."ProductId";

    ELSIF TG_OP = 'UPDATE' THEN
        INSERT INTO "InventoryMovements"("ProductId", "QtyChange", "SourceType", "SourceId")
        VALUES (OLD."ProductId",  OLD."Quantity", 'Sale', OLD."InvoiceId");
        INSERT INTO "InventoryMovements"("ProductId", "QtyChange", "SourceType", "SourceId")
        VALUES (NEW."ProductId", -NEW."Quantity", 'Sale', NEW."InvoiceId");
        v_product_id := NEW."ProductId";

    ELSIF TG_OP = 'DELETE' THEN
        INSERT INTO "InventoryMovements"("ProductId", "QtyChange", "SourceType", "SourceId")
        VALUES (OLD."ProductId", OLD."Quantity", 'Sale', OLD."InvoiceId");
        v_product_id := OLD."ProductId";
    END IF;

    UPDATE "Products"
    SET "Stock" = COALESCE((
        SELECT SUM("QtyChange") FROM "InventoryMovements" WHERE "ProductId" = v_product_id
    ), 0)
    WHERE "ProductId" = v_product_id;

    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER tr_SalesDetails_MoveStock
AFTER INSERT OR UPDATE OR DELETE ON "SalesDetails"
FOR EACH ROW EXECUTE FUNCTION fn_SalesDetails_MoveStock();

-- ============================================================
-- Trigger: PurchaseReceiptDetails → InventoryMovements → Products.Stock
-- ============================================================
CREATE OR REPLACE FUNCTION fn_PurchaseReceiptDetails_MoveStock()
RETURNS TRIGGER AS $$
DECLARE
    v_product_id VARCHAR(50);
BEGIN
    IF TG_OP = 'INSERT' THEN
        INSERT INTO "InventoryMovements"("ProductId", "QtyChange", "SourceType", "SourceId")
        VALUES (NEW."ProductId",  NEW."QuantityReceived", 'PurchaseReceipt', NEW."ReceiptId");
        v_product_id := NEW."ProductId";

    ELSIF TG_OP = 'UPDATE' THEN
        INSERT INTO "InventoryMovements"("ProductId", "QtyChange", "SourceType", "SourceId")
        VALUES (OLD."ProductId", -OLD."QuantityReceived", 'PurchaseReceipt', OLD."ReceiptId");
        INSERT INTO "InventoryMovements"("ProductId", "QtyChange", "SourceType", "SourceId")
        VALUES (NEW."ProductId",  NEW."QuantityReceived", 'PurchaseReceipt', NEW."ReceiptId");
        v_product_id := NEW."ProductId";

    ELSIF TG_OP = 'DELETE' THEN
        INSERT INTO "InventoryMovements"("ProductId", "QtyChange", "SourceType", "SourceId")
        VALUES (OLD."ProductId", -OLD."QuantityReceived", 'PurchaseReceipt', OLD."ReceiptId");
        v_product_id := OLD."ProductId";
    END IF;

    UPDATE "Products"
    SET "Stock" = COALESCE((
        SELECT SUM("QtyChange") FROM "InventoryMovements" WHERE "ProductId" = v_product_id
    ), 0)
    WHERE "ProductId" = v_product_id;

    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER tr_PurchaseReceiptDetails_MoveStock
AFTER INSERT OR UPDATE OR DELETE ON "PurchaseReceiptDetails"
FOR EACH ROW EXECUTE FUNCTION fn_PurchaseReceiptDetails_MoveStock();

-- ============================================================
-- Vistas
-- ============================================================
CREATE OR REPLACE VIEW v_SalesSummary AS
SELECT
    s."InvoiceId",
    s."InvoiceDate",
    s."Status",
    c."Name"         AS "ClientName",
    c."DocumentType",
    c."DocumentID",
    s."Subtotal",
    s."Tax",
    s."Total",
    s."ExchangeRate"
FROM "Sales" s
JOIN "Clients" c ON c."ClientId" = s."ClientId";

CREATE OR REPLACE VIEW v_SalesLines AS
SELECT
    s."InvoiceId",
    s."InvoiceDate",
    c."Name"    AS "ClientName",
    d."ProductId",
    p."Name"    AS "ProductName",
    d."Quantity",
    d."UnitPrice",
    d."LineTotal"
FROM "SalesDetails" d
JOIN "Sales"    s ON s."InvoiceId"  = d."InvoiceId"
JOIN "Products" p ON p."ProductId" = d."ProductId"
JOIN "Clients"  c ON c."ClientId"  = s."ClientId";

CREATE OR REPLACE VIEW v_CurrentStock AS
SELECT
    p."ProductId",
    p."Name",
    COALESCE(SUM(m."QtyChange"), 0) AS "Stock"
FROM "Products" p
LEFT JOIN "InventoryMovements" m ON m."ProductId" = p."ProductId"
GROUP BY p."ProductId", p."Name";

/* ============================================================
   Notas de uso
   ============================================================
   - Crear la BD antes de ejecutar este script:
       CREATE DATABASE "CPI";
   - Ejecutar este script conectado a la BD CPI:
       psql -h localhost -U postgres -d CPI -f schema.sql
   - Para recalcular totales de una venta:
       CALL sp_Sales_RecalculateTotals(1, 4050.1234);
   - Los triggers mantienen Products.Stock sincronizado
     vía InventoryMovements automáticamente.
*/
