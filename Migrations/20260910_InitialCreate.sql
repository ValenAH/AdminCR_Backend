-- Migration: 20260910_InitialCreate.sql
-- Objetivo: crear la estructura base del sistema para Supabase/PostgreSQL.
-- La clasificación principal de los productos se hace con category.

CREATE TABLE IF NOT EXISTS "role" (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS identification_type (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS category (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS customer (
    id SERIAL PRIMARY KEY,
    name VARCHAR(150) NOT NULL,
    telephone VARCHAR(30),
    email VARCHAR(150),
    address TEXT,
    identification_type_id INTEGER NOT NULL,
    identification_number VARCHAR(50) NOT NULL,
    CONSTRAINT fk_customer_identification_type
        FOREIGN KEY (identification_type_id) REFERENCES identification_type(id)
);

CREATE TABLE IF NOT EXISTS product (
    id SERIAL PRIMARY KEY,
    name VARCHAR(150) NOT NULL,
    description TEXT,
    price NUMERIC(12,2) NOT NULL,
    unit_cost NUMERIC(12,2) NOT NULL,
    category_id INTEGER NOT NULL,
    enable BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT fk_product_category
        FOREIGN KEY (category_id) REFERENCES category(id)
);

CREATE TABLE IF NOT EXISTS sale_status (
    id SERIAL PRIMARY KEY,
    status VARCHAR(80) NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS sale (
    id SERIAL PRIMARY KEY,
    consecutive VARCHAR(50) NOT NULL UNIQUE,
    sale_date TIMESTAMPTZ NOT NULL,
    customer_id INTEGER NOT NULL,
    delivery_date TIMESTAMPTZ,
    sale_status_id INTEGER NOT NULL,
    total_amount NUMERIC(12,2) NOT NULL,
    is_credit BOOLEAN NOT NULL DEFAULT FALSE,
    CONSTRAINT fk_sale_customer
        FOREIGN KEY (customer_id) REFERENCES customer(id),
    CONSTRAINT fk_sale_status
        FOREIGN KEY (sale_status_id) REFERENCES sale_status(id)
);

CREATE TABLE IF NOT EXISTS sale_details (
    id SERIAL PRIMARY KEY,
    sale_id INTEGER NOT NULL,
    product_id INTEGER NOT NULL,
    quantity INTEGER NOT NULL,
    amount NUMERIC(12,2) NOT NULL,
    tax NUMERIC(12,2) DEFAULT 0,
    CONSTRAINT fk_sale_details_sale
        FOREIGN KEY (sale_id) REFERENCES sale(id),
    CONSTRAINT fk_sale_details_product
        FOREIGN KEY (product_id) REFERENCES product(id)
);

CREATE TABLE IF NOT EXISTS payment_method (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    enable BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS payment (
    id SERIAL PRIMARY KEY,
    sale_id INTEGER NOT NULL,
    payment_method_id INTEGER NOT NULL,
    amount NUMERIC(12,2) NOT NULL,
    date TIMESTAMPTZ NOT NULL,
    CONSTRAINT fk_payment_sale
        FOREIGN KEY (sale_id) REFERENCES sale(id),
    CONSTRAINT fk_payment_method
        FOREIGN KEY (payment_method_id) REFERENCES payment_method(id)
);

CREATE TABLE IF NOT EXISTS quota (
    id SERIAL PRIMARY KEY,
    sale_id INTEGER NOT NULL,
    amount NUMERIC(12,2) NOT NULL,
    date TIMESTAMPTZ NOT NULL,
    is_payed BOOLEAN NOT NULL DEFAULT FALSE,
    CONSTRAINT fk_quota_sale
        FOREIGN KEY (sale_id) REFERENCES sale(id)
);

CREATE TABLE IF NOT EXISTS "user" (
    id SERIAL PRIMARY KEY,
    name VARCHAR(150),
    username VARCHAR(100) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    role_id INTEGER NOT NULL,
    enable BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT fk_user_role
        FOREIGN KEY (role_id) REFERENCES "role"(id)
);

INSERT INTO "role" (name)
VALUES ('Administrador'), ('Vendedor')
ON CONFLICT (name) DO NOTHING;

INSERT INTO category (name)
VALUES ('Colchones'), ('Basecamas'), ('Accesorios de cama')
ON CONFLICT (name) DO NOTHING;

INSERT INTO identification_type (name)
VALUES ('Cédula'), ('RUC'), ('Pasaporte')
ON CONFLICT (name) DO NOTHING;

INSERT INTO sale_status (status)
VALUES ('Pendiente'), ('Entregada'), ('Finalizada')
ON CONFLICT (status) DO NOTHING;

INSERT INTO payment_method (name, enable)
VALUES ('Efectivo', TRUE), ('Tarjeta', TRUE), ('Transferencia', TRUE), ('Crédito', TRUE)
ON CONFLICT (name) DO NOTHING;
