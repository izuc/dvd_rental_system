USE Master
GO
IF EXISTS(SELECT * FROM sys.databases WHERE name='DVDEzy')
DROP DATABASE DVDEzy
GO
CREATE DATABASE DVDEzy
GO
USE DVDEzy
GO

CREATE TABLE Customer(
	customer_id int IDENTITY(1,1) NOT NULL,
	first_name varchar(15) NOT NULL,
	last_name varchar(15) NOT NULL,
	street_address varchar(50) NOT NULL,
	billing_address varchar(50) NULL,
	postcode int NOT NULL,
	gender int NOT NULL,
	PRIMARY KEY(customer_id)
);

CREATE TABLE DVD(
	dvd_id int IDENTITY(1,1) NOT NULL,
	title varchar(50) NOT NULL,
	director varchar(30) NOT NULL,
	year int NOT NULL,
	sale_price money NOT NULL,
	rental_price money NOT NULL,
	PRIMARY KEY (dvd_id)
); 

CREATE TABLE DVDCopy(
	copy_id int IDENTITY(1,1) NOT NULL,
	dvd_id int NOT NULL,
	barcode nchar(10) NOT NULL UNIQUE,
	status int NOT NULL,
	PRIMARY KEY (copy_id),
	CONSTRAINT FK_DVDCopy_DVD FOREIGN KEY(dvd_id)
		REFERENCES DVD (dvd_id)
		ON UPDATE CASCADE
		ON DELETE CASCADE
); 

CREATE TABLE CreditCard(
	card_id int IDENTITY(1,1) NOT NULL,
	customer_id int NOT NULL,
	card_holder varchar(50) NULL,
	card_number nchar(16) NOT NULL UNIQUE,
	PRIMARY KEY (card_id),
	CONSTRAINT FK_CreditCard_Customer FOREIGN KEY(customer_id)
		REFERENCES Customer (customer_id)
		ON UPDATE CASCADE
		ON DELETE CASCADE
); 

CREATE TABLE TransactionCart(
	transaction_id int IDENTITY(1,1) NOT NULL,
	customer_id int NOT NULL,
	payment_type int NOT NULL,
	card_id int NULL,
	transaction_date datetime NOT NULL,
	PRIMARY KEY (transaction_id),
	CONSTRAINT FK_Transaction_CreditCard FOREIGN KEY(card_id)
		REFERENCES CreditCard (card_id)
		ON UPDATE NO ACTION
		ON DELETE NO ACTION,
	CONSTRAINT FK_Transaction_Customer FOREIGN KEY(customer_id)
		REFERENCES Customer (customer_id)
		ON UPDATE CASCADE
		ON DELETE CASCADE
);

CREATE TABLE TransactionItem(
	transaction_id int NOT NULL,
	copy_id int NOT NULL,
	type int NOT NULL,
	PRIMARY KEY (transaction_id, copy_id),
	CONSTRAINT FK_TransactionItem_DVDCopy FOREIGN KEY(copy_id)
		REFERENCES DVDCopy (copy_id)
		ON UPDATE NO ACTION
		ON DELETE NO ACTION,
	CONSTRAINT FK_TransactionItem_Transaction FOREIGN KEY(transaction_id)
		REFERENCES TransactionCart (transaction_id)
		ON UPDATE CASCADE
		ON DELETE CASCADE
);

SET IDENTITY_INSERT Customer ON;

BEGIN TRANSACTION;
INSERT INTO Customer(customer_id, first_name, last_name, street_address, billing_address, postcode, gender)
SELECT 1, N'Kaylee', N'Frye', N'5 Bunk, Serenity', N'', 3663, 1 UNION ALL
SELECT 2, N'Malcolm', N'Reynolds', N'234 Ranch, Shadow', N'', 4552, 0 UNION ALL
SELECT 3, N'River', N'Tam', N'Tam Estate, Osiris', N'', 6523, 1 UNION ALL
SELECT 4, N'Simon', N'Tam', N'Tam Estate, Osiris', N'', 6523, 0 UNION ALL
SELECT 5, N'Jayne', N'Cobb', N'Jaynestown, Higgins Moon', N'', 4335, 0 UNION ALL
SELECT 6, N'Zoe', N'Washburne', N'2 Bunk, Serenity', N'', 3663, 1 UNION ALL
SELECT 7, N'Hoban', N'Washburne', N'2 Bunk, Serenity', N'', 3663, 0 UNION ALL
SELECT 8, N'Inara', N'Serra', N'1 Shuttle, Serenity', N'', 3663, 1 UNION ALL
SELECT 9, N'Shepherd', N'Book', N'3 Bunk, Serenity', N'', 3663, 0 UNION ALL
SELECT 10, N'Summer', N'Glau', N'1 Firefly Road, Space', N'', 4550, 1
COMMIT;
RAISERROR (N'Customer: Insert Done!', 10, 1) WITH NOWAIT;
GO

SET IDENTITY_INSERT Customer OFF;

SET IDENTITY_INSERT DVD ON;

BEGIN TRANSACTION;
INSERT INTO DVD(dvd_id, title, director, year, sale_price, rental_price)
SELECT 1, N'Inception', N'Christopher Nolan', 2010, 18.0000, 4.0000 UNION ALL
SELECT 2, N'Star Wars: Episode V', N'Irvin Kershner', 1977, 10.0000, 3.0000 UNION ALL
SELECT 3, N'The Matrix', N'The Wachowski Brothers', 1999, 8.0000, 3.0000 UNION ALL
SELECT 4, N'Terminator 2: Judgment Day', N'James Cameron', 1991, 8.0000, 3.0000 UNION ALL
SELECT 5, N'Alien', N'Ridley Scott', 1979, 7.0000, 4.0000 UNION ALL
SELECT 6, N'Aliens', N'James Cameron', 1986, 7.0000, 4.0000 UNION ALL
SELECT 7, N'Back to the Future', N'Robert Zemeckis', 1955, 8.0000, 4.0000 UNION ALL
SELECT 8, N'2001: A Space Odyssey', N'Stanley Kubrick', 1968, 9.0000, 3.0000 UNION ALL
SELECT 9, N'Blade Runner', N'Ridley Scott', 1982, 9.0000, 3.0000 UNION ALL
SELECT 10, N'The Thing', N'John Carpenter', 1982, 9.0000, 3.0000 UNION ALL
SELECT 11, N'District 9', N'Neill Blomkamp', 2009, 9.0000, 3.0000
COMMIT;
RAISERROR (N'DVD: Insert Done!', 10, 1) WITH NOWAIT;
GO

SET IDENTITY_INSERT DVD OFF;

SET IDENTITY_INSERT CreditCard ON;

BEGIN TRANSACTION;
INSERT INTO CreditCard(card_id, customer_id, card_holder, card_number)
SELECT 1, 2, N'Malcolm Reynolds', N'4444444444444444' UNION ALL
SELECT 2, 10, N'Ms. Summer Glau', N'1111111111111111' UNION ALL
SELECT 3, 10, N'Ms. Summer Glau', N'2222222222222222'
COMMIT;
RAISERROR (N'CreditCard: Insert Done!', 10, 1) WITH NOWAIT;
GO

SET IDENTITY_INSERT CreditCard OFF;

SET IDENTITY_INSERT DVDCopy ON;

BEGIN TRANSACTION;
INSERT INTO DVDCopy(copy_id, dvd_id, barcode, status)
SELECT 1, 1, N'00001     ', 0 UNION ALL
SELECT 2, 1, N'00002     ', 0 UNION ALL
SELECT 4, 1, N'00003     ', 0 UNION ALL
SELECT 5, 1, N'00004     ', 0 UNION ALL
SELECT 6, 1, N'00005     ', 0 UNION ALL
SELECT 7, 2, N'00006     ', 1 UNION ALL
SELECT 8, 2, N'00007     ', 0 UNION ALL
SELECT 9, 2, N'00008     ', 0 UNION ALL
SELECT 10, 2, N'00009     ', 0 UNION ALL
SELECT 11, 2, N'00010     ', 0 UNION ALL
SELECT 12, 3, N'00011     ', 0 UNION ALL
SELECT 13, 3, N'00012     ', 0 UNION ALL
SELECT 14, 3, N'00013     ', 0 UNION ALL
SELECT 15, 3, N'00014     ', 0 UNION ALL
SELECT 16, 3, N'00015     ', 0 UNION ALL
SELECT 17, 4, N'00016     ', 0 UNION ALL
SELECT 18, 4, N'00017     ', 0 UNION ALL
SELECT 19, 4, N'00018     ', 0 UNION ALL
SELECT 20, 4, N'00019     ', 0 UNION ALL
SELECT 21, 4, N'00020     ', 0 UNION ALL
SELECT 22, 5, N'00026     ', 0 UNION ALL
SELECT 23, 5, N'00027     ', 0 UNION ALL
SELECT 24, 5, N'00028     ', 1 UNION ALL
SELECT 25, 5, N'00029     ', 0 UNION ALL
SELECT 26, 5, N'00030     ', 0 UNION ALL
SELECT 27, 6, N'00031     ', 0 UNION ALL
SELECT 28, 6, N'00032     ', 0 UNION ALL
SELECT 29, 6, N'00033     ', 0 UNION ALL
SELECT 30, 6, N'00034     ', 0 UNION ALL
SELECT 31, 6, N'00035     ', 0 UNION ALL
SELECT 32, 7, N'00036     ', 0 UNION ALL
SELECT 33, 7, N'00037     ', 0 UNION ALL
SELECT 34, 7, N'00038     ', 0 UNION ALL
SELECT 35, 7, N'00039     ', 0 UNION ALL
SELECT 36, 7, N'00040     ', 0 UNION ALL
SELECT 37, 8, N'00041     ', 0 UNION ALL
SELECT 38, 8, N'00042     ', 0 UNION ALL
SELECT 39, 8, N'00043     ', 0 UNION ALL
SELECT 40, 8, N'00044     ', 0 UNION ALL
SELECT 41, 8, N'00045     ', 0 UNION ALL
SELECT 42, 9, N'00046     ', 1 UNION ALL
SELECT 43, 9, N'00047     ', 0 UNION ALL
SELECT 44, 9, N'00048     ', 0 UNION ALL
SELECT 45, 9, N'00049     ', 0 UNION ALL
SELECT 46, 9, N'00050     ', 0 UNION ALL
SELECT 47, 10, N'00051     ', 1 UNION ALL
SELECT 48, 10, N'00052     ', 0 UNION ALL
SELECT 49, 10, N'00053     ', 0 UNION ALL
SELECT 50, 10, N'00054     ', 0 UNION ALL
SELECT 51, 10, N'00055     ', 0
COMMIT;
RAISERROR (N'DVDCopy: Insert Done!', 10, 1) WITH NOWAIT;
GO

BEGIN TRANSACTION;
INSERT INTO DVDCopy(copy_id, dvd_id, barcode, status)
SELECT 52, 11, N'00056     ', 0 UNION ALL
SELECT 53, 11, N'00057     ', 0 UNION ALL
SELECT 54, 11, N'00058     ', 0 UNION ALL
SELECT 55, 11, N'00059     ', 0 UNION ALL
SELECT 56, 11, N'00060     ', 0
COMMIT;
RAISERROR (N'DVDCopy: Insert Done!', 10, 1) WITH NOWAIT;
GO

SET IDENTITY_INSERT DVDCopy OFF;

SET IDENTITY_INSERT TransactionCart ON;

BEGIN TRANSACTION;
INSERT INTO TransactionCart(transaction_id, customer_id, payment_type, card_id, transaction_date)
SELECT 1, 2, 1, 1, '20120503 00:00:00.000' UNION ALL
SELECT 2, 10, 1, 2, '20120503 00:00:00.000'
COMMIT;
RAISERROR (N'TransactionCart: Insert Done!', 10, 1) WITH NOWAIT;
GO

SET IDENTITY_INSERT TransactionCart OFF;

BEGIN TRANSACTION;
INSERT INTO TransactionItem(transaction_id, copy_id, type)
SELECT 1, 7, 0 UNION ALL
SELECT 1, 24, 0 UNION ALL
SELECT 1, 42, 0 UNION ALL
SELECT 2, 47, 0
COMMIT;
RAISERROR (N'TransactionItem: Insert Done!', 10, 1) WITH NOWAIT;
GO