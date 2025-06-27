-- Script for creating and inserting data

-- CREATE DATABASE StationeryAndSuppliesDatabase;

-- SHOW DATABASES; 
-- 
-- USE StationeryAndSuppliesDatabase; 
-- 
-- SHOW TABLES; 

-- Creating Tables 

-- CREATE TABLE categories (
-- 	CategoryID INT PRIMARY KEY AUTO_INCREMENT,
-- 	ParentID INT NULL,
-- 	Name VARCHAR(100) NOT NULL
-- ); 

-- ALTER TABLE categories 
-- ADD  ImageUrl VARCHAR(200) NULL; 

-- CREATE TABLE products (
-- 	ProductID INT PRIMARY KEY AUTO_INCREMENT, 
-- 	Name VARCHAR(200) NOT NULL, 
-- 	Descripttion TEXT NULL, 
-- 	Price DECIMAL(10,2) CHECK (Price >= 0.01),
-- 	Stock INT NOT NULL CHECK (Stock >= 0),
-- 	Status ENUM('active', 'inactive', 'archived'),
-- 	ImageUrl VARCHAR(200) NULL, 
-- 	CreatedAt DATE DEFAULT (CURRENT_DATE()), 
-- 	CategoryID INT NOT NULL, 
-- 
-- 	FOREIGN KEY (CategoryID) REFERENCES categories (CategoryID)			
-- ); 


-- Need to add role column later when adding identity
-- CREATE TABLE users (
-- 	UserID INT PRIMARY KEY AUTO_INCREMENT, 
-- 	Name VARCHAR(50) NOT NULL,
-- 	Email VARCHAR(100) NOT NULL UNIQUE, 
-- 	Phone VARCHAR(100) NULL, 
-- 	CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP, 
-- 	PasswordHash VARCHAR(255) NOT NULL
-- ); 


-- CREATE TABLE reviews (
-- 	ReviewID INT PRIMARY KEY AUTO_INCREMENT, 
-- 	UserID INT NOT NULL, 
-- 	ProductID INT NOT NULL, 
-- 	Rating TINYINT NOT NULL CHECK (Rating > 0 AND Rating <= 5), 
-- 	Comment TEXT NULL, 
-- 	CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
-- 	
-- 	FOREIGN KEY (UserID) REFERENCES users (UserID), 
-- 	FOREIGN KEY (ProductID) REFERENCES products (ProductID)
-- ); 


-- CREATE TABLE cart (
-- 	CartID INT PRIMARY KEY AUTO_INCREMENT, 
-- 	UserID INT NOT NULL UNIQUE, 
-- 	CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
-- 	
-- 	FOREIGN KEY (UserID) REFERENCES users (UserID)
-- ); 


-- CREATE TABLE cart_items (
-- 	CartItemID INT PRIMARY KEY AUTO_INCREMENT, 
-- 	CartID INT NOT NULL, 
-- 	ProductID INT NOT NULL, 
-- 	Quantity INT NOT NULL CHECK (Quantity > 0), 
-- 	
-- 	FOREIGN KEY (CartID) REFERENCES cart (CartID), 
-- 	FOREIGN KEY (ProductID) REFERENCES products (ProductID)
-- ); 



-- CREATE TABLE orders (
-- 	OrderID INT PRIMARY KEY AUTO_INCREMENT, 
-- 	UserID INT NOT NULL, 
-- 	TotalAmount DECIMAL(10,2) CHECK (TotalAmount >= 0.00),
-- 	Status ENUM('pending', 'shipped', 'cancelled', 'delivered'), 
-- 	OrderDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP, 
-- 	ShippingAddress TEXT NOT NULL,
-- 	ShippingCost DECIMAL(10,2) NOT NULL DEFAULT 0.00,
-- 	
-- 	FOREIGN KEY (UserID) REFERENCES users (UserID)
-- ); 



-- CREATE TABLE order_items (
-- 	OrderItemID INT PRIMARY KEY AUTO_INCREMENT, 
-- 	OrderID INT NOT NULL, 
-- 	ProductID INT NOT NULL, 
-- 	Quantity INT NOT NULL CHECK (Quantity > 0), 
-- 	UnitPrice DECIMAL(10,2) NOT NULL CHECK (UnitPrice >= 0.00), 
-- 	
-- 	FOREIGN KEY (OrderID) REFERENCES orders (OrderID), 
-- 	FOREIGN KEY (ProductID) REFERENCES products (ProductID)
-- ); 


-- CREATE TABLE payments (
-- 	PaymentID INT PRIMARY KEY AUTO_INCREMENT, 
-- 	OrderID INT NOT NULL UNIQUE, 
-- 	Amount DECIMAL(10,2) NOT NULL CHECK (Amount >= 0.00), 
-- 	PymentMethod VARCHAR(50) NOT NULL, 
-- 	Status ENUM('success', 'failed'), 
-- 	PayedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
-- 	
-- 	FOREIGN KEY (OrderID) REFERENCES orders (OrderID)
-- ); 




-- Inserting Data

-- categories data

-- Top-level categories
-- INSERT INTO categories (ParentID, Name) VALUES
-- (NULL, 'Writing Instruments'),
-- (NULL, 'Paper Products'),
-- (NULL, 'Desk Accessories'),
-- (NULL, 'Art Supplies'), 
-- (NULL, 'Office Electronics'); 
-- 
-- -- Subcategories of 'Writing Instruments' (assume ID 1)
-- INSERT INTO categories (ParentID, Name) VALUES
-- (1, 'Pens'),
-- (1, 'Pencils'),
-- (1, 'Markers');
-- 
-- -- Subcategories of 'Paper Products' 
-- INSERT INTO categories (ParentID, Name) VALUES
-- (2, 'Notebooks'),
-- (2, 'Sticky Notes'),
-- (2, 'Printer Paper');
-- 
-- -- Subcategories of 'Desk Accessories' 
-- INSERT INTO categories (ParentID, Name) VALUES
-- (3, 'Staplers'),
-- (3, 'Paper Clips'),
-- (3, 'Tape Dispensers');
-- 
-- -- Subcategories of 'Art Supplies' 
-- INSERT INTO categories (ParentID, Name) VALUES
-- (4, 'Paint Brushes'),
-- (4, 'Sketchbooks'),
-- (4, 'Color Pencils');
-- 
-- -- Subcategories of 'Office Electronics' 
-- INSERT INTO categories (ParentID, Name) VALUES 
-- (5, 'Calculators'),
-- (5, 'Laminators'),
-- (5, 'Label Makers');



-- Product data 
-- 
-- -- Insert 10 Products
-- INSERT INTO products (Name, Descripttion, Price, Stock, Status, ImageUrl, CategoryID) VALUES
-- ('Pilot G2 Gel Pen', 'Smooth writing gel pen with fine point.', 1.99, 150, 'active', NULL, 6),
-- ('BIC Round Stic Pen', 'Affordable ballpoint pen, black ink.', 0.49, 300, 'inactive', NULL, 6),
-- ('Parker Jotter Pen', 'Classic stainless steel ballpoint pen.', 13.49, 25, 'archived', NULL, 6),
-- ('Lamy Safari Fountain Pen', 'Stylish fountain pen with refillable ink.', 28.50, 40, 'active', NULL, 6),
-- ('Dixon Ticonderoga Pencil', 'Classic wood pencil for writing.', 0.39, 180, 'active', NULL, 7),
-- ('Staedtler Mechanical Pencil', '0.5mm pencil with ergonomic grip.', 4.99, 120, 'active', NULL, 7),
-- ('Pentel GraphGear 1000', 'Professional drafting pencil.', 10.99, 70, 'active', NULL, 7),
-- ('Sharpie Fine Point', 'Permanent marker, assorted colors.', 1.89, 120, 'active', NULL, 8),
-- ('Expo Dry Erase Marker', 'Whiteboard marker, low odor.', 6.79, 90, 'inactive', NULL, 8),
-- ('Crayola Washable Marker Set', 'Set of 10 colorful markers.', 4.79, 110, 'active', NULL, 8);
-- 
-- -- Insert 10 Products
-- INSERT INTO products (Name, Descripttion, Price, Stock, Status, ImageUrl, CategoryID) VALUES
-- ('Spiral Notebook A5', 'College-ruled 100-sheet notebook.', 3.29, 180, 'active', NULL, 9),
-- ('Moleskine Classic Notebook', 'Hardcover, ruled, black.', 20.49, 60, 'archived', NULL, 9),
-- ('Eco Notebook', 'Recycled paper, eco-friendly.', 2.99, 100, 'active', NULL, 9),
-- ('Composition Notebook', 'Wide ruled, 100 sheets.', 1.85, 90, 'active', NULL, 9),
-- ('Post-it Notes 3x3', 'Assorted colors, pack of 5.', 4.79, 100, 'active', NULL, 10),
-- ('Post-it Tabs', 'Colorful, easy-to-use tabs.', 3.95, 75, 'inactive', NULL, 10),
-- ('A4 Copy Paper', '500 sheets, 80gsm.', 6.29, 200, 'active', NULL, 11),
-- ('Legal Size Paper', 'White, 8.5x14 inches.', 7.10, 145, 'active', NULL, 11),
-- ('Colored Printer Paper', 'Pastel colors, 100 sheets.', 5.39, 70, 'archived', NULL, 11),
-- ('Swingline Stapler', 'Standard stapler for office use.', 7.25, 85, 'active', NULL, 12);
-- 
-- -- Insert 10 Products
-- INSERT INTO products (Name, Descripttion, Price, Stock, Status, ImageUrl, CategoryID) VALUES
-- ('Mini Stapler', 'Compact, great for travel.', 3.45, 160, 'active', NULL, 12),
-- ('Jumbo Paper Clips', '100 count, assorted colors.', 2.15, 300, 'active', NULL, 13),
-- ('Metal Paper Clips', 'Rust-resistant finish.', 2.39, 80, 'inactive', NULL, 13),
-- ('Scotch Tape Dispenser', 'Weighted, black finish.', 4.89, 100, 'active', NULL, 14),
-- ('Double-Sided Tape Roll', 'Ideal for gift wrapping.', 2.99, 90, 'inactive', NULL, 14),
-- ('Artist Brush Set', '10-piece assorted brush sizes.', 8.29, 50, 'active', NULL, 15),
-- ('Watercolor Brush Pack', 'Soft bristles, wood handles.', 6.55, 75, 'active', NULL, 15),
-- ('A4 Sketchpad', '60-sheet acid-free paper.', 5.99, 80, 'archived', NULL, 16),
-- ('Canson Sketchbook', 'Hardcover, elastic closure.', 9.29, 60, 'active', NULL, 16),
-- ('Crayola Colored Pencils 24-pack', 'Pre-sharpened, bright colors.', 5.25, 120, 'active', NULL, 17);
-- 
-- -- Insert 10 Products
-- INSERT INTO products (Name, Descripttion, Price, Stock, Status, ImageUrl, CategoryID) VALUES
-- ('Prismacolor Premier Set', 'Artist-quality color pencils.', 30.99, 35, 'archived', NULL, 17),
-- ('Casio FX-991EX', 'Scientific calculator, solar-powered.', 19.29, 45, 'active', NULL, 18),
-- ('TI-84 Plus', 'Graphing calculator with USB cable.', 124.99, 20, 'active', NULL, 18),
-- ('Pocket Calculator', 'Basic functions, 8-digit display.', 5.95, 60, 'inactive', NULL, 18),
-- ('Scotch Thermal Laminator', 'Quick warm-up, 9-inch input.', 31.49, 30, 'active', NULL, 19),
-- ('Fellowes Saturn 3i', 'Advanced heat and jam control.', 46.89, 10, 'inactive', NULL, 19),
-- ('DYMO LabelWriter 450', 'Thermal label printer.', 91.99, 25, 'active', NULL, 20),
-- ('Brother PT-D210', 'Handheld label maker with templates.', 36.79, 40, 'active', NULL, 20),
-- ('Mini Bluetooth Label Printer', 'Connects to phone app.', 26.50, 65, 'active', NULL, 20),
-- ('Industrial Label Maker', 'Heavy-duty, great for warehouse use.', 78.00, 20, 'active', NULL, 20);
-- 
-- 
-- -- Insert Final 10 Products
-- INSERT INTO products (Name, Descripttion, Price, Stock, Status, ImageUrl, CategoryID) VALUES
-- ('Sticky Note Cube', 'Multi-color cube of sticky notes.', 6.99, 75, 'active', NULL, 10),
-- ('Translucent Sticky Notes', 'Transparent sticky notes for books.', 4.25, 90, 'active', NULL, 10),
-- ('Mini Sketchbook', 'Pocket-sized with textured paper.', 3.99, 85, 'active', NULL, 16),
-- ('Ergonomic Stapler', 'Reduced effort stapling.', 8.49, 60, 'active', NULL, 12),
-- ('Wooden Pencil Set', 'Natural wood, pre-sharpened.', 2.79, 140, 'active', NULL, 7),
-- ('Neon Markers Pack', 'Bright fluorescent ink.', 4.59, 50, 'inactive', NULL, 8),
-- ('Aesthetic Notebook', 'Minimalist cover, 120 pages.', 6.25, 95, 'active', NULL, 9),
-- ('Label Tape Refill', 'Compatible with Brother label makers.', 7.15, 100, 'active', NULL, 20),
-- ('Art Eraser Pack', '3 erasers, ideal for sketches.', 1.99, 120, 'active', NULL, 15),
-- ('Portable Calculator', 'Palm-sized and efficient.', 9.45, 70, 'active', NULL, 18);




-- Users Data 
-- INSERT INTO users (Name, Email, Phone) VALUES
-- ('Alice Johnson', 'alice.johnson@example.com', '555-123-4567'),
-- ('Bob Smith', 'bob.smith@example.com', '555-234-5678'),
-- ('Charlie Brown', 'charlie.brown@example.com', NULL),
-- ('Diana Prince', 'diana.prince@example.com', '555-345-6789'),
-- ('Ethan Hunt', 'ethan.hunt@example.com', NULL),
-- ('Fiona Gallagher', 'fiona.g@example.com', '555-456-7890'),
-- ('George Martin', 'george.martin@example.com', '555-567-8901'),
-- ('Hannah Wells', 'hannah.w@example.com', NULL),
-- ('Ian Fleming', 'ian.fleming@example.com', '555-678-9012'),
-- ('Julia Roberts', 'julia.roberts@example.com', '555-789-0123');




-- reviews data 
-- Note: ProductID 17 intentionally skipped
-- INSERT INTO reviews (UserID, ProductID, Rating, Comment) VALUES
-- (3, 1, 5, 'Fantastic pen, smooth ink flow.'),
-- (1, 2, 4, 'Good value for the price.'),
-- (6, 3, 3, 'Nice design, but a bit expensive.'),
-- (9, 4, 5, 'My favorite fountain pen!'),
-- (2, 5, 5, 'Classic pencil, writes very well.'),
-- (8, 6, 4, 'Comfortable grip and accurate lines.'),
-- (10, 7, 2, 'Gets the job done, but wears quickly.'),
-- (5, 8, 5, 'Perfect for precision drawing.'),
-- (4, 9, 5, 'Great color options, lasts long.'),
-- (7, 10, 4, 'Good for office whiteboards.'),
-- (1, 11, 5, 'Kids love these markers.'),
-- (6, 12, 4, 'Reliable notebook for school.'),
-- (2, 13, 5, 'Excellent quality and feel.'),
-- (3, 14, 3, 'Nice concept but binding could be stronger.'),
-- (5, 15, 4, 'Solid build, serves its purpose.'),
-- (8, 16, 4, 'Tabs are super handy.'),
-- (10, 17, 5, 'Crisp white pages, prints perfectly.'),
-- (6, 18, 4, 'Good for legal documents.'),
-- (1, 19, 3, 'Color is nice, but paper curls a bit.'),
-- (7, 20, 5, 'Staples easily and never jams.'),
-- (4, 21, 4, 'Great for light-duty stapling.'),
-- (2, 22, 3, 'Colorful but sometimes bends easily.'),
-- (9, 23, 5, 'Very durable and smooth.'),
-- (5, 24, 4, 'Sticks well and easy to use.'),
-- (8, 25, 4, 'Great for gift wrapping.'),
-- (10, 26, 5, 'Perfect for acrylics and watercolors.'),
-- (3, 27, 4, 'Nice texture, holds pigment well.'),
-- (6, 28, 5, 'Best sketchpad I\'ve used.'),
-- (1, 29, 4, 'Compact and stylish.'),
-- (7, 30, 5, 'Kids enjoy coloring with these.'),
-- (2, 31, 5, 'Rich colors and smooth shading.'),
-- (4, 32, 5, 'Best calculator I\'ve owned.'),
-- (9, 33, 4, 'Perfect for advanced math classes.'),
-- (6, 34, 3, 'Works fine but feels cheap.'),
-- (5, 35, 5, 'Great quality for the price.'),
-- (8, 36, 4, 'Quick warm-up and easy to use.'),
-- (10, 37, 4, 'Very reliable machine.'),
-- (3, 38, 5, 'Fast label printing, saves time.'),
-- (1, 39, 5, 'Excellent for organizing files.'),
-- (7, 40, 3, 'App can be buggy, but prints well.'),
-- (2, 41, 4, 'Stapler is a bit stiff at first.'),
-- (4, 42, 5, 'Love the vibrant sticky notes!'),
-- (9, 43, 5, 'Great brush set for beginners.'),
-- (5, 44, 4, 'Does the job nicely.'),
-- (8, 45, 3, 'Slightly thin paper but usable.'),
-- (10, 46, 5, 'Trendy and durable.'),
-- (6, 47, 4, 'Ink dries fast, doesn\'t smear.'),
-- (1, 48, 2, 'Label roll jammed once.'),
-- (7, 49, 4, 'Good display and ergonomic design.');




-- Orders data 
-- INSERT INTO orders (UserID, TotalAmount, Status, OrderDate, ShippingAddress) VALUES
-- (1, 87.94, 'shipped', '2024-08-13 14:25:00', '12 High Street, Manchester M1 1AA, United Kingdom'); 


-- Order items data 
-- INSERT INTO order_items (OrderID, ProductID, Quantity, UnitPrice) VALUES
-- (9, 3, 5, 13.49),  
-- (9, 12, 1, 20.49); 













-- To add CATEGORIES Images folder path


-- To add common folders path
-- START TRANSACTION; 
-- 
-- UPDATE categories 
-- SET ImageUrl = 'images/products/'
-- WHERE ParentID IS NOT NULL; 


-- To add common parent folder path
-- START TRANSACTION;
-- 
-- UPDATE categories 
-- SET ImageUrl = CONCAT(ImageUrl, 'OfficeElectronics/')
-- WHERE 
-- 	ParentID IS NOT NULL
-- 		AND 
-- 	ParentID = 5; 
-- 
-- SELECT *
-- FROM categories ; 




-- To add category image path

-- SELECT *
-- FROM categories ; 
-- 
-- 
-- 
-- START TRANSACTION; 
-- 
-- UPDATE categories 
-- SET ImageUrl = CONCAT(ImageUrl, 'Pens/PensCategoryImage.jpeg')
-- WHERE CategoryID = 6; 
-- 
-- 
-- UPDATE categories 
-- SET ImageUrl = CONCAT(ImageUrl, 'Pencils/PencilCategoryImage.jpg')
-- WHERE CategoryID = 7; 
-- 
-- 
-- UPDATE categories 
-- SET ImageUrl = CONCAT(ImageUrl, 'Markers/MarkersCategoryImage.jpg')
-- WHERE CategoryID = 8; 
-- 
-- 
-- SELECT *
-- FROM categories ; 




-- START TRANSACTION; 
-- 
-- UPDATE categories 
-- SET ImageUrl = CONCAT(ImageUrl, 'Notebooks/NotebooksCategoryImage.jpg')
-- WHERE CategoryID = 9; 
-- 
-- 
-- UPDATE categories 
-- SET ImageUrl = CONCAT(ImageUrl, 'StickyNotes/StickyNotesCategoryImage.webp')
-- WHERE CategoryID = 10; 
-- 
-- 
-- UPDATE categories 
-- SET ImageUrl = CONCAT(ImageUrl, 'PrinterPaper/PrinterPaperCategoryImage.jpg')
-- WHERE CategoryID = 11; 
-- 
-- SELECT *
-- FROM categories ; 
 


-- START TRANSACTION; 
-- 
-- UPDATE categories 
-- SET ImageUrl = CONCAT(ImageUrl, 'Staplers/StaplersCategoryImage.jpg')
-- WHERE CategoryID = 12; 
-- 
-- 
-- UPDATE categories 
-- SET ImageUrl = CONCAT(ImageUrl, 'PaperClips/PaperClipsCategoryImage.avif')
-- WHERE CategoryID = 13; 
-- 
-- 
-- UPDATE categories 
-- SET ImageUrl = CONCAT(ImageUrl, 'TapeDispensers/TapeDispensersCategoryImage.avif')
-- WHERE CategoryID = 14; 
-- 
-- 
-- SELECT *
-- FROM categories ; 
-- 




-- START TRANSACTION; 
-- 
-- UPDATE categories 
-- SET ImageUrl = CONCAT(ImageUrl, 'PaintBrushes/PaintBrushesCategoryImage.jpg')
-- WHERE CategoryID = 15; 
-- 
-- 
-- UPDATE categories 
-- SET ImageUrl = CONCAT(ImageUrl, 'Sketchbooks/SketchbooksCategoryImage.webp')
-- WHERE CategoryID = 16; 
-- 
-- 
-- UPDATE categories 
-- SET ImageUrl = CONCAT(ImageUrl, 'ColorPencils/ColorPencilsCategoryImage.jpg')
-- WHERE CategoryID = 17; 
-- 
-- 
-- SELECT *
-- FROM categories ; 




-- START TRANSACTION; 
-- 
-- UPDATE categories 
-- SET ImageUrl = CONCAT(ImageUrl, 'Calculators/CalculatorsCategoryImage.jpg')
-- WHERE CategoryID = 18; 
-- 
-- 
-- UPDATE categories 
-- SET ImageUrl = CONCAT(ImageUrl, 'Laminators/LaminatorsCategoryImage.jpg')
-- WHERE CategoryID = 19; 
-- 
-- 
-- UPDATE categories 
-- SET ImageUrl = CONCAT(ImageUrl, 'LabelMakers/LabelMakersCategoryImage.jpg')
-- WHERE CategoryID = 20; 
-- 
-- SELECT *
-- FROM categories ; 
-- 






-- Updating products with parent category of WritingInstruments
-- 
-- START TRANSACTION; 
-- 
-- UPDATE products P
-- INNER JOIN categories C	
-- 	ON P.CategoryID = C.CategoryID
-- INNER JOIN categories C2 
-- 	ON C.ParentID = C2.CategoryID
-- SET P.ImageUrl = 'images/products/WritingInstruments/'
-- WHERE C2.Name = 'Writing Instruments'; 
-- 
--  
-- 
-- 
-- START TRANSACTION; 
-- 
-- UPDATE products P
-- INNER JOIN categories C	
-- 	ON P.CategoryID = C.CategoryID
-- INNER JOIN categories C2 
-- 	ON C.ParentID = C2.CategoryID
-- SET P.ImageUrl = CONCAT(P.ImageUrl, 'Pens/')
-- WHERE C.Name = 'Pens'; 
-- 
-- 
-- 
-- START TRANSACTION; 
-- 
-- UPDATE products P
-- INNER JOIN categories C	
-- 	ON P.CategoryID = C.CategoryID
-- INNER JOIN categories C2 
-- 	ON C.ParentID = C2.CategoryID
-- SET P.ImageUrl = CONCAT(P.ImageUrl, 'Pencils/')
-- WHERE C.Name = 'Pencils'; 
-- 
-- 
-- 
-- START TRANSACTION; 
-- 
-- UPDATE products P
-- INNER JOIN categories C	
-- 	ON P.CategoryID = C.CategoryID
-- INNER JOIN categories C2 
-- 	ON C.ParentID = C2.CategoryID
-- SET P.ImageUrl = CONCAT(P.ImageUrl, 'Markers/')
-- WHERE C.Name = 'Markers'; 



-- START TRANSACTION; 
-- 
-- UPDATE products 
-- SET ImageUrl = CONCAT(ImageUrl, 'PilotG2GelPen.jpg')
-- WHERE ProductID = 1; 
-- 
-- UPDATE products 
-- SET ImageUrl = CONCAT(ImageUrl, 'BICRoundSticPen.jpg')
-- WHERE ProductID = 2; 
-- 
-- UPDATE products 
-- SET ImageUrl = CONCAT(ImageUrl, 'ParkerJotterPen.webp')
-- WHERE ProductID = 3; 
-- 
-- UPDATE products 
-- SET ImageUrl = CONCAT(ImageUrl, 'LamySafariFountainPen.jpg')
-- WHERE ProductID = 4; 



-- START TRANSACTION; 
-- 
-- 
-- UPDATE products 
-- SET ImageUrl = CONCAT(ImageUrl, 'WoodenPencilSet.webp')
-- WHERE Name = 'Wooden Pencil Set'; 
-- 
-- UPDATE products 
-- SET ImageUrl = CONCAT(ImageUrl, 'PentelGraphGear1000.webp')
-- WHERE Name = 'Pentel GraphGear 1000'; 
-- 
-- UPDATE products 
-- SET ImageUrl = CONCAT(ImageUrl, 'StaedtlerMechanicalPencil.jpg')
-- WHERE Name = 'Staedtler Mechanical Pencil'; 
-- 
-- UPDATE products 
-- SET ImageUrl = CONCAT(ImageUrl, 'DixonTiconderogaPencil.jpg')
-- WHERE Name = 'Dixon Ticonderoga Pencil'; 
-- 



-- START TRANSACTION;
-- 
-- UPDATE products 
-- SET ImageUrl = CONCAT(ImageUrl, 'NeonMarkersPack.webp')
-- WHERE Name = 'Neon Markers Pack'; 
-- 
-- UPDATE products 
-- SET ImageUrl = CONCAT(ImageUrl, 'CrayolaWashableMarkerSet.jpg')
-- WHERE Name = 'Crayola Washable Marker Set'; 
-- 
-- UPDATE products 
-- SET ImageUrl = CONCAT(ImageUrl, 'ExpoDryEraseMarker.jpg')
-- WHERE Name = 'Expo Dry Erase Marker'; 
-- 
-- UPDATE products 
-- SET ImageUrl = CONCAT(ImageUrl, 'SharpieFinePoint.jpg')
-- WHERE Name = 'Sharpie Fine Point'; 





-- Replace space in category name with dash

-- SELECT *
-- FROM categories C ; 

-- START TRANSACTION; 
-- 
-- UPDATE categories 
-- SET Name = REPLACE(Name, ' ', '-'); 







-- Queries

-- categories queries
DESCRIBE categories; 

SELECT *
FROM categories; 

SELECT 
	C2.Name AS ParentCategory,
	C1.Name AS Category
FROM categories C1
INNER JOIN categories C2
	ON C1.ParentID = C2.CategoryID; 

SELECT 
	C.Name AS Categories
FROM categories C
WHERE C.ParentID IS NULL; 

SELECT 
	C.Name, 
	count(*) AS NumberOfProducts
FROM categories C
INNER JOIN products P 
	ON C.CategoryID  = P.CategoryID
GROUP BY C.Name; 


SELECT 
	C2.Name AS ParentCategory,
	count(*) AS NumberOfProducts
FROM categories C1
INNER JOIN categories C2
	ON C1.ParentID = C2.CategoryID
INNER JOIN products P
	ON C1.CategoryID = P.CategoryID
GROUP BY C2.Name; 



SELECT *
FROM categories
WHERE ParentID IS NOT NULL
ORDER BY CategoryID 
LIMIT 10 
OFFSET 0; 



-- To Check Process list
SELECT *
FROM performance_schema.processlist; 






-- products queries

SELECT *
FROM products P; 



SELECT 
	C2.CategoryID AS ParentCategoryID, 
	C2.Name AS ParentCategoryName,
	C.CategoryID, 
	C.Name AS CategoryName, 
	P.ProductID,
	P.Name AS ProductName
FROM categories C
INNER JOIN categories C2
	ON C.ParentID = C2.CategoryID
INNER JOIN products P 
	ON C.CategoryID = P.CategoryID
ORDER BY C2.CategoryID , P.ProductID; 



SELECT 
	C2.CategoryID AS ParentCategoryID, 
	C2.Name AS ParentCategoryName,
	C.CategoryID, 
	C.Name AS CategoryName, 
	P.ProductID,
	P.Name AS ProductName
FROM categories C
INNER JOIN categories C2
	ON C.ParentID = C2.CategoryID
INNER JOIN products P 
	ON C.CategoryID = P.CategoryID
WHERE P.Status = 'active'
ORDER BY C2.CategoryID , P.ProductID; 



SELECT 
	C2.Name AS ParentCategoryName,
	C.Name AS CategoryName, 
	P.ProductID, 
	P.Name AS ProductName, 
	P.Stock, 
	P.Status, 
	P.ImageUrl
FROM categories C
INNER JOIN categories C2
	ON C.ParentID = C2.CategoryID
INNER JOIN products P 
	ON C.CategoryID = P.CategoryID
ORDER BY C2.CategoryID , P.ProductID; 


-- old stock 300

-- START TRANSACTION; 
-- 
-- UPDATE products 
-- SET Stock = 0
-- WHERE ProductID = 2; 
-- 
-- UPDATE products 
-- SET Status = 'inactive'
-- WHERE ProductID = 2; 
-- 
-- 
-- COMMIT; 





-- Adding user data to test delete operation

INSERT INTO reviews (UserID, ProductID, Rating, Comment)
VALUES 
(24, 3, 4, 'Very reliable product, will buy again.'),
(24, 7, 2, 'Not satisfied with the quality.');



-- Order 
INSERT INTO orders (UserID, TotalAmount, Status, ShippingAddress)
VALUES (24, 26.47, 'delivered', '10 Downing Street, London, UK');


-- Order Items
-- Get orders id before adding order items 
INSERT INTO order_items (OrderID, ProductID, Quantity, UnitPrice)
VALUES 
(10, 2, 1, 15.99),
(10, 5, 2, 5.24);


SELECT *
FROM orders O; 

SELECT *
FROM order_items OI; 


INSERT INTO payments (OrderID, Amount, PymentMethod, Status)
VALUES (10, 26.47, 'card', 'success');



-- cart 
INSERT INTO cart (UserID) VALUES (24);

-- Get cart id before inserting cart items 
INSERT INTO cart_items (CartID, ProductID, Quantity) VALUES 
(1, 4, 1),     
(1, 6, 3); 


SELECT *
FROM cart; 









SELECT 
	U.UserID, 
	U.Name AS UserName,
	U.Email,
	O.OrderID, 
	OI.OrderItemID,
	P.PaymentID,
	P.Status, 
	C.CartID, 
	CI.CartItemID
FROM users U
INNER JOIN orders O
	ON U.UserID = O.UserID
INNER JOIN order_items OI
	ON O.OrderID = OI.OrderID
INNER JOIN payments P
	ON P.OrderID = O.OrderID
INNER JOIN cart C
	ON C.UserID = U.UserID
INNER JOIN cart_items CI
	ON C.CartID = CI.CartID; 





















-- quering products by page 
SELECT 
	P.ProductID, 
	P.Name
FROM products P
ORDER BY P.ProductID
LIMIT 10 -- page size
OFFSET 40; -- skip 40 items 



SELECT 
	C2.Name AS ParentCategoryName, 
	C.Name AS CategoryName, 
	P.ProductID, 
	P.Name AS ProductName, 
	P.ImageUrl
FROM products P
INNER JOIN categories C	
	ON P.CategoryID = C.CategoryID
INNER JOIN categories C2 
	ON C.ParentID = C2.CategoryID 
-- WHERE C2.Name = 'Writing Instruments'
ORDER BY C2.Name DESC;  








SELECT 
	C2.Name AS ParentCategoryName, 
	C.Name AS CategoryName, 
	P.ProductID, 
	P.Name AS ProductName, 
	P.ImageUrl
FROM products P
INNER JOIN categories C	
	ON P.CategoryID = C.CategoryID
INNER JOIN categories C2 
	ON C.ParentID = C2.CategoryID 
-- WHERE C2.Name = 'Writing Instruments'
ORDER BY C2.Name DESC;  



-- DELETE FROM products ; 

-- ALTER TABLE products AUTO_INCREMENT = 1; 



SELECT 
	P.ProductID, 
	P.Name, 
	P.Price
FROM products P 
WHERE P.Name LIKE '%pen%'; 







-- users queries 
DESCRIBE users; 


SELECT *
FROM users U; 


SELECT 
	U.UserID, 
	U.Name, 
	U.Email, 
	R.Rating, 
	R.Comment, 
	P.Name AS ProductName
FROM users U 
INNER JOIN reviews R
	ON U.UserID = R.UserID
INNER JOIN products P
	ON P.ProductID = R.ProductID; 











-- Cart queries
DESCRIBE cart; 

SELECT *
FROM cart ; 



-- Order queries 

SELECT * 
FROM orders; 


SELECT 
	O.OrderID, 
	U.Name AS UserName,
	O.TotalAmount,
	P.Name AS ProductName,
	OI.Quantity, 
	OI.UnitPrice,
	O.Status
FROM orders O
INNER JOIN order_items OI 
	ON O.OrderID = OI.OrderID
INNER JOIN products P
	ON OI.ProductID = P.ProductID
INNER JOIN users U
	ON O.UserID = U.UserID
WHERE O.OrderID = 1; 


SELECT 
	O.OrderID, 
	U.Name AS UserName,
	O.TotalAmount,
	SUM(OI.Quantity) AS TotalProducts,
	O.Status
FROM orders O
INNER JOIN order_items OI 
	ON O.OrderID = OI.OrderID
INNER JOIN products P
	ON OI.ProductID = P.ProductID
INNER JOIN users U
	ON O.UserID = U.UserID
GROUP BY O.OrderID; 





SELECT 
	O.OrderID, 
	P.Name AS ProductName,
	P.Price AS Price,
	OI.Quantity,
	U.Name AS UserName,
	O.OrderDate,
	O.Status,
	O.TotalAmount
FROM orders O
INNER JOIN order_items OI 
	ON O.OrderID = OI.OrderID
INNER JOIN products P
	ON OI.ProductID = P.ProductID
INNER JOIN users U
	ON O.UserID = U.UserID; 




-- Orders items queries 

SELECT *
FROM order_items; 











