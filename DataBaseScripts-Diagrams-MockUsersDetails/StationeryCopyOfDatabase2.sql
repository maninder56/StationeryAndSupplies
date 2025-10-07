CREATE DATABASE  IF NOT EXISTS `stationeryandsuppliesdatabase` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `stationeryandsuppliesdatabase`;
-- MySQL dump 10.13  Distrib 8.0.42, for Win64 (x86_64)
--
-- Host: localhost    Database: stationeryandsuppliesdatabase
-- ------------------------------------------------------
-- Server version	8.0.42

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `cart`
--

DROP TABLE IF EXISTS `cart`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cart` (
  `CartID` int NOT NULL AUTO_INCREMENT,
  `UserID` int NOT NULL,
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`CartID`),
  UNIQUE KEY `UserID` (`UserID`),
  UNIQUE KEY `UserID_2` (`UserID`),
  CONSTRAINT `cart_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cart`
--

LOCK TABLES `cart` WRITE;
/*!40000 ALTER TABLE `cart` DISABLE KEYS */;
INSERT INTO `cart` VALUES (3,1,'2025-06-30 17:24:39');
/*!40000 ALTER TABLE `cart` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cart_items`
--

DROP TABLE IF EXISTS `cart_items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cart_items` (
  `CartItemID` int NOT NULL AUTO_INCREMENT,
  `CartID` int NOT NULL,
  `ProductID` int NOT NULL,
  `Quantity` int NOT NULL,
  PRIMARY KEY (`CartItemID`),
  KEY `CartID` (`CartID`),
  KEY `ProductID` (`ProductID`),
  CONSTRAINT `cart_items_ibfk_1` FOREIGN KEY (`CartID`) REFERENCES `cart` (`CartID`),
  CONSTRAINT `cart_items_ibfk_2` FOREIGN KEY (`ProductID`) REFERENCES `products` (`ProductID`),
  CONSTRAINT `cart_items_chk_1` CHECK ((`Quantity` > 0))
) ENGINE=InnoDB AUTO_INCREMENT=121 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cart_items`
--

LOCK TABLES `cart_items` WRITE;
/*!40000 ALTER TABLE `cart_items` DISABLE KEYS */;
/*!40000 ALTER TABLE `cart_items` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `CategoryID` int NOT NULL AUTO_INCREMENT,
  `ParentID` int DEFAULT NULL,
  `Name` varchar(100) NOT NULL,
  `ImageUrl` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`CategoryID`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (1,NULL,'Writing-Instruments',NULL),(2,NULL,'Paper-Products',NULL),(3,NULL,'Desk-Accessories',NULL),(4,NULL,'Art-Supplies',NULL),(5,NULL,'Office-Electronics',NULL),(6,1,'Pens','images/products/WritingInstruments/Pens/PensCategoryImage.jpg'),(7,1,'Pencils','images/products/WritingInstruments/Pencils/PencilCategoryImage.jpg'),(8,1,'Markers','images/products/WritingInstruments/Markers/MarkersCategoryImage.jpg'),(9,2,'Notebooks','images/products/PaperProducts/Notebooks/NotebooksCategoryImage.jpg'),(10,2,'Sticky-Notes','images/products/PaperProducts/StickyNotes/StickyNotesCategoryImage.webp'),(11,2,'Printer-Paper','images/products/PaperProducts/PrinterPaper/PrinterPaperCategoryImage.jpg'),(12,3,'Staplers','images/products/DeskAccessories/Staplers/StaplersCategoryImage.jpg'),(13,3,'Paper-Clips','images/products/DeskAccessories/PaperClips/PaperClipsCategoryImage.avif'),(14,3,'Tape-Dispensers','images/products/DeskAccessories/TapeDispensers/TapeDispensersCategoryImage.avif'),(15,4,'Paint-Brushes','images/products/ArtSupplies/PaintBrushes/PaintBrushesCategoryImage.jpg'),(16,4,'Sketchbooks','images/products/ArtSupplies/Sketchbooks/SketchbooksCategoryImage.webp'),(17,4,'Color-Pencils','images/products/ArtSupplies/ColorPencils/ColorPencilsCategoryImage.jpg'),(18,5,'Calculators','images/products/OfficeElectronics/Calculators/CalculatorsCategoryImage.jpg'),(19,5,'Laminators','images/products/OfficeElectronics/Laminators/LaminatorsCategoryImage.jpg'),(20,5,'Label-Makers','images/products/OfficeElectronics/LabelMakers/LabelMakersCategoryImage.jpg');
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `order_items`
--

DROP TABLE IF EXISTS `order_items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `order_items` (
  `OrderItemID` int NOT NULL AUTO_INCREMENT,
  `OrderID` int NOT NULL,
  `ProductID` int NOT NULL,
  `Quantity` int NOT NULL,
  `UnitPrice` decimal(10,2) NOT NULL,
  PRIMARY KEY (`OrderItemID`),
  KEY `OrderID` (`OrderID`),
  KEY `ProductID` (`ProductID`),
  CONSTRAINT `order_items_ibfk_1` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`OrderID`),
  CONSTRAINT `order_items_ibfk_2` FOREIGN KEY (`ProductID`) REFERENCES `products` (`ProductID`),
  CONSTRAINT `order_items_chk_1` CHECK ((`Quantity` > 0)),
  CONSTRAINT `order_items_chk_2` CHECK ((`UnitPrice` >= 0.00))
) ENGINE=InnoDB AUTO_INCREMENT=88 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order_items`
--

LOCK TABLES `order_items` WRITE;
/*!40000 ALTER TABLE `order_items` DISABLE KEYS */;
INSERT INTO `order_items` VALUES (22,8,5,2,0.39),(23,8,9,1,6.79),(24,9,3,5,13.49),(25,9,12,1,20.49),(86,21,18,1,7.10),(87,21,12,7,20.49);
/*!40000 ALTER TABLE `order_items` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orders` (
  `OrderID` int NOT NULL AUTO_INCREMENT,
  `UserID` int NOT NULL,
  `TotalAmount` decimal(10,2) DEFAULT NULL,
  `Status` enum('pending','shipped','cancelled','delivered') DEFAULT NULL,
  `OrderDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ShippingAddress` text NOT NULL,
  `ShippingCost` decimal(10,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`OrderID`),
  KEY `UserID` (`UserID`),
  CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`),
  CONSTRAINT `orders_chk_1` CHECK ((`TotalAmount` >= 0.00))
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (8,1,7.18,'pending','2025-05-29 14:25:00','12 High Street, Manchester M1 1AA, United Kingdom',0.00),(9,1,87.94,'shipped','2024-08-13 14:25:00','12 High Street, Manchester M1 1AA, United Kingdom',0.00),(21,36,153.52,'shipped','2025-07-02 17:00:12','Fake address ',2.99);
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `payments`
--

DROP TABLE IF EXISTS `payments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `payments` (
  `PaymentID` int NOT NULL AUTO_INCREMENT,
  `OrderID` int NOT NULL,
  `Amount` decimal(10,2) NOT NULL,
  `PaymentMethod` varchar(50) NOT NULL,
  `Status` enum('success','failed') DEFAULT NULL,
  `PayedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`PaymentID`),
  UNIQUE KEY `OrderID` (`OrderID`),
  CONSTRAINT `payments_ibfk_1` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`OrderID`),
  CONSTRAINT `payments_chk_1` CHECK ((`Amount` >= 0.00))
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `payments`
--

LOCK TABLES `payments` WRITE;
/*!40000 ALTER TABLE `payments` DISABLE KEYS */;
INSERT INTO `payments` VALUES (12,21,153.52,'Card','success','2025-07-02 17:00:12');
/*!40000 ALTER TABLE `payments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products` (
  `ProductID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `Descripttion` text,
  `Price` decimal(10,2) NOT NULL,
  `Stock` int NOT NULL,
  `Status` enum('active','inactive','archived') DEFAULT NULL,
  `ImageUrl` varchar(200) DEFAULT NULL,
  `CreatedAt` date DEFAULT (curdate()),
  `CategoryID` int NOT NULL,
  PRIMARY KEY (`ProductID`),
  KEY `CategoryID` (`CategoryID`),
  CONSTRAINT `products_ibfk_1` FOREIGN KEY (`CategoryID`) REFERENCES `categories` (`CategoryID`),
  CONSTRAINT `products_chk_1` CHECK ((`Price` >= 0.01)),
  CONSTRAINT `products_chk_2` CHECK ((`Stock` >= 0))
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES (1,'Pilot G2 Gel Pen','Smooth writing gel pen with fine point.',1.99,150,'active','images/products/WritingInstruments/Pens/PilotG2GelPen.jpg','2025-06-02',6),(2,'BIC Round Stic Pen','Affordable ballpoint pen, black ink.',0.49,0,'inactive','images/products/WritingInstruments/Pens/BICRoundSticPen.jpg','2025-06-02',6),(3,'Parker Jotter Pen','Classic stainless steel ballpoint pen.',13.49,25,'active','images/products/WritingInstruments/Pens/ParkerJotterPen.webp','2025-06-02',6),(4,'Lamy Safari Fountain Pen','Stylish fountain pen with refillable ink.',28.50,40,'active','images/products/WritingInstruments/Pens/LamySafariFountainPen.jpg','2025-06-02',6),(5,'Dixon Ticonderoga Pencil','Classic wood pencil for writing.',0.39,180,'active','images/products/WritingInstruments/Pencils/DixonTiconderogaPencil.jpg','2025-06-02',7),(6,'Staedtler Mechanical Pencil','0.5mm pencil with ergonomic grip.',4.99,120,'active','images/products/WritingInstruments/Pencils/StaedtlerMechanicalPencil.jpg','2025-06-02',7),(7,'Pentel GraphGear 1000','Professional drafting pencil.',10.99,70,'active','images/products/WritingInstruments/Pencils/PentelGraphGear1000.webp','2025-06-02',7),(8,'Sharpie Fine Point','Permanent marker, assorted colors.',1.89,120,'active','images/products/WritingInstruments/Markers/SharpieFinePoint.jpg','2025-06-02',8),(9,'Expo Dry Erase Marker','Whiteboard marker, low odor.',6.79,90,'active','images/products/WritingInstruments/Markers/ExpoDryEraseMarker.jpg','2025-06-02',8),(10,'Crayola Washable Marker Set','Set of 10 colorful markers.',4.79,110,'active','images/products/WritingInstruments/Markers/CrayolaWashableMarkerSet.jpg','2025-06-02',8),(11,'Spiral Notebook A5','College-ruled 100-sheet notebook.',3.29,180,'active','images/products/PaperProducts/Notebooks/SpiralNotebookA5.jpg','2025-06-02',9),(12,'Moleskine Classic Notebook','Hardcover, ruled, black.',20.49,60,'active','images/products/PaperProducts/Notebooks/MoleskineClassicNotebook.jpg','2025-06-02',9),(13,'Eco Notebook','Recycled paper, eco-friendly.',2.99,100,'active','images/products/PaperProducts/Notebooks/EcoNotebook.jpg','2025-06-02',9),(14,'Composition Notebook','Wide ruled, 100 sheets.',1.85,90,'active','images/products/PaperProducts/Notebooks/CompositionNotebook.jpg','2025-06-02',9),(15,'Post-it Notes 3x3','Assorted colors, pack of 5.',4.79,100,'active','images/products/PaperProducts/StickyNotes/Post-itNotes3x3.jpg','2025-06-02',10),(16,'Post-it Tabs','Colorful, easy-to-use tabs.',3.95,75,'active','images/products/PaperProducts/StickyNotes/Post-itTabs.jpg','2025-06-02',10),(17,'A4 Copy Paper','500 sheets, 80gsm.',6.29,200,'active','images/products/PaperProducts/PrinterPaper/A4CopyPaper.jpg','2025-06-02',11),(18,'Legal Size Paper','White, 8.5x14 inches.',7.10,145,'active','images/products/PaperProducts/PrinterPaper/LegalSizePaper.jpg','2025-06-02',11),(19,'Colored Printer Paper','Pastel colors, 100 sheets.',5.39,70,'active','images/products/PaperProducts/PrinterPaper/ColoredPrinterPaper.jpg','2025-06-02',11),(20,'Swingline Stapler','Standard stapler for office use.',7.25,85,'active','images/products/DeskAccessories/Staplers/SwinglineStapler.jpg','2025-06-02',12),(21,'Mini Stapler','Compact, great for travel.',3.45,160,'active','images/products/DeskAccessories/Staplers/MiniStapler.jpg','2025-06-02',12),(22,'Jumbo Paper Clips','100 count, assorted colors.',2.15,300,'active','images/products/DeskAccessories/PaperClips/JumboPaperClips.jpg','2025-06-02',13),(23,'Metal Paper Clips','Rust-resistant finish.',2.39,80,'active','images/products/DeskAccessories/PaperClips/MetalPaperClips.jpg','2025-06-02',13),(24,'Scotch Tape Dispenser','Weighted, black finish.',4.89,100,'active','images/products/DeskAccessories/TapeDispensers/ScotchTapeDispenser.jpg','2025-06-02',14),(25,'Double-Sided Tape Roll','Ideal for gift wrapping.',2.99,90,'active','images/products/DeskAccessories/TapeDispensers/Double-SidedTapeRoll.jpg','2025-06-02',14),(26,'Artist Brush Set','10-piece assorted brush sizes.',8.29,50,'active','images/products/ArtSupplies/PaintBrushes/ArtistBrushSet.jpg','2025-06-02',15),(27,'Watercolor Brush Pack','Soft bristles, wood handles.',6.55,75,'active','images/products/ArtSupplies/PaintBrushes/WatercolorBrushPack.jpg','2025-06-02',15),(28,'A4 Sketchpad','60-sheet acid-free paper.',5.99,80,'active','images/products/ArtSupplies/Sketchbooks/A4Sketchpad.jpg','2025-06-02',16),(29,'Canson Sketchbook','Hardcover, elastic closure.',9.29,60,'active','images/products/ArtSupplies/Sketchbooks/CansonSketchbook.jpg','2025-06-02',16),(30,'Crayola Colored Pencils 24-pack','Pre-sharpened, bright colors.',5.25,120,'active','images/products/ArtSupplies/ColorPencils/CrayolaColoredPencils24-pack.jpg','2025-06-02',17),(31,'Prismacolor Premier Set','Artist-quality color pencils.',30.99,35,'active','images/products/ArtSupplies/ColorPencils/PrismacolorPremierSet.jpg','2025-06-02',17),(32,'Casio FX-991EX','Scientific calculator, solar-powered.',19.29,45,'active','images/products/OfficeElectronics/Calculators/CasioFX-991EX.jpg','2025-06-02',18),(33,'TI-84 Plus','Graphing calculator with USB cable.',124.99,20,'active','images/products/OfficeElectronics/Calculators/TI-84Plus.jpg','2025-06-02',18),(34,'Pocket Calculator','Basic functions, 8-digit display.',5.95,60,'active','images/products/OfficeElectronics/Calculators/PocketCalculator.jpg','2025-06-02',18),(35,'Scotch Thermal Laminator','Quick warm-up, 9-inch input.',31.49,30,'active','images/products/OfficeElectronics/Laminators/ScotchThermalLaminator.jpg','2025-06-02',19),(36,'Fellowes Saturn 3i','Advanced heat and jam control.',46.89,10,'active','images/products/OfficeElectronics/Laminators/FellowesSaturn3i.jpg','2025-06-02',19),(37,'DYMO LabelWriter 450','Thermal label printer.',91.99,25,'active','images/products/OfficeElectronics/LabelMakers/DYMOLabelWriter450.jpg','2025-06-02',20),(38,'Brother PT-D210','Handheld label maker with templates.',36.79,40,'active','images/products/OfficeElectronics/LabelMakers/BrotherPT-D210.jpg','2025-06-02',20),(39,'Mini Bluetooth Label Printer','Connects to phone app.',26.50,65,'active','images/products/OfficeElectronics/LabelMakers/MiniBluetoothLabelPrinter.jpg','2025-06-02',20),(40,'Industrial Label Maker','Heavy-duty, great for warehouse use.',78.00,20,'active','images/products/OfficeElectronics/LabelMakers/IndustrialLabelMaker.jpg','2025-06-02',20),(41,'Sticky Note Cube','Multi-color cube of sticky notes.',6.99,75,'active','images/products/PaperProducts/StickyNotes/StickyNoteCube.jpg','2025-06-02',10),(42,'Translucent Sticky Notes','Transparent sticky notes for books.',4.25,90,'active','images/products/PaperProducts/StickyNotes/TranslucentStickyNotes.jpg','2025-06-02',10),(43,'Mini Sketchbook','Pocket-sized with textured paper.',3.99,85,'active','images/products/ArtSupplies/Sketchbooks/MiniSketchbook.jpg','2025-06-02',16),(44,'Ergonomic Stapler','Reduced effort stapling.',8.49,60,'active','images/products/DeskAccessories/Staplers/ErgonomicStapler.jpg','2025-06-02',12),(45,'Wooden Pencil Set','Natural wood, pre-sharpened.',2.79,140,'active','images/products/WritingInstruments/Pencils/WoodenPencilSet.webp','2025-06-02',7),(46,'Neon Markers Pack','Bright fluorescent ink.',4.59,50,'active','images/products/WritingInstruments/Markers/NeonMarkersPack.webp','2025-06-02',8),(47,'Aesthetic Notebook','Minimalist cover, 120 pages.',6.25,95,'active','images/products/PaperProducts/Notebooks/AestheticNotebook.jpg','2025-06-02',9),(48,'Label Tape Refill','Compatible with Brother label makers.',7.15,100,'active','images/products/OfficeElectronics/LabelMakers/LabelTapeRefill.jpg','2025-06-02',20),(49,'Art Eraser Pack','3 erasers, ideal for sketches.',1.99,120,'active','images/products/ArtSupplies/PaintBrushes/ArtEraserPack.jpg','2025-06-02',15),(50,'Portable Calculator','Palm-sized and efficient.',9.45,70,'active','images/products/OfficeElectronics/Calculators/PortableCalculator.jpg','2025-06-02',18);
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reviews`
--

DROP TABLE IF EXISTS `reviews`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reviews` (
  `ReviewID` int NOT NULL AUTO_INCREMENT,
  `UserID` int NOT NULL,
  `ProductID` int NOT NULL,
  `Rating` tinyint NOT NULL,
  `Comment` text,
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ReviewID`),
  KEY `UserID` (`UserID`),
  KEY `ProductID` (`ProductID`),
  CONSTRAINT `reviews_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `users` (`UserID`),
  CONSTRAINT `reviews_ibfk_2` FOREIGN KEY (`ProductID`) REFERENCES `products` (`ProductID`),
  CONSTRAINT `reviews_chk_1` CHECK (((`Rating` > 0) and (`Rating` <= 5)))
) ENGINE=InnoDB AUTO_INCREMENT=54 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reviews`
--

LOCK TABLES `reviews` WRITE;
/*!40000 ALTER TABLE `reviews` DISABLE KEYS */;
INSERT INTO `reviews` VALUES (1,3,1,5,'Fantastic pen, smooth ink flow.','2025-06-02 10:06:38'),(2,1,2,4,'Good value for the price.','2025-06-02 10:06:38'),(3,6,3,3,'Nice design, but a bit expensive.','2025-06-02 10:06:38'),(4,9,4,5,'My favorite fountain pen!','2025-06-02 10:06:38'),(5,2,5,5,'Classic pencil, writes very well.','2025-06-02 10:06:38'),(6,8,6,4,'Comfortable grip and accurate lines.','2025-06-02 10:06:38'),(7,10,7,2,'Gets the job done, but wears quickly.','2025-06-02 10:06:38'),(8,5,8,5,'Perfect for precision drawing.','2025-06-02 10:06:38'),(9,4,9,5,'Great color options, lasts long.','2025-06-02 10:06:38'),(10,7,10,4,'Good for office whiteboards.','2025-06-02 10:06:38'),(11,1,11,5,'Kids love these markers.','2025-06-02 10:06:38'),(12,6,12,4,'Reliable notebook for school.','2025-06-02 10:06:38'),(13,2,13,5,'Excellent quality and feel.','2025-06-02 10:06:38'),(14,3,14,3,'Nice concept but binding could be stronger.','2025-06-02 10:06:38'),(15,5,15,4,'Solid build, serves its purpose.','2025-06-02 10:06:38'),(16,8,16,4,'Tabs are super handy.','2025-06-02 10:06:38'),(17,10,17,5,'Crisp white pages, prints perfectly.','2025-06-02 10:06:38'),(18,6,18,4,'Good for legal documents.','2025-06-02 10:06:38'),(19,1,19,3,'Color is nice, but paper curls a bit.','2025-06-02 10:06:38'),(20,7,20,5,'Staples easily and never jams.','2025-06-02 10:06:38'),(21,4,21,4,'Great for light-duty stapling.','2025-06-02 10:06:38'),(22,2,22,3,'Colorful but sometimes bends easily.','2025-06-02 10:06:38'),(23,9,23,5,'Very durable and smooth.','2025-06-02 10:06:38'),(24,5,24,4,'Sticks well and easy to use.','2025-06-02 10:06:38'),(25,8,25,4,'Great for gift wrapping.','2025-06-02 10:06:38'),(26,10,26,5,'Perfect for acrylics and watercolors.','2025-06-02 10:06:38'),(27,3,27,4,'Nice texture, holds pigment well.','2025-06-02 10:06:38'),(28,6,28,5,'Best sketchpad I\'ve used.','2025-06-02 10:06:38'),(29,1,29,4,'Compact and stylish.','2025-06-02 10:06:38'),(30,7,30,5,'Kids enjoy coloring with these.','2025-06-02 10:06:38'),(31,2,31,5,'Rich colors and smooth shading.','2025-06-02 10:06:38'),(32,4,32,5,'Best calculator I\'ve owned.','2025-06-02 10:06:38'),(33,9,33,4,'Perfect for advanced math classes.','2025-06-02 10:06:38'),(34,6,34,3,'Works fine but feels cheap.','2025-06-02 10:06:38'),(35,5,35,5,'Great quality for the price.','2025-06-02 10:06:38'),(36,8,36,4,'Quick warm-up and easy to use.','2025-06-02 10:06:38'),(37,10,37,4,'Very reliable machine.','2025-06-02 10:06:38'),(38,3,38,5,'Fast label printing, saves time.','2025-06-02 10:06:38'),(39,1,39,5,'Excellent for organizing files.','2025-06-02 10:06:38'),(40,7,40,3,'App can be buggy, but prints well.','2025-06-02 10:06:38'),(41,2,41,4,'Stapler is a bit stiff at first.','2025-06-02 10:06:38'),(42,4,42,5,'Love the vibrant sticky notes!','2025-06-02 10:06:38'),(43,9,43,5,'Great brush set for beginners.','2025-06-02 10:06:38'),(44,5,44,4,'Does the job nicely.','2025-06-02 10:06:38'),(45,8,45,3,'Slightly thin paper but usable.','2025-06-02 10:06:38'),(46,10,46,5,'Trendy and durable.','2025-06-02 10:06:38'),(47,6,47,4,'Ink dries fast, doesn\'t smear.','2025-06-02 10:06:38'),(48,1,48,2,'Label roll jammed once.','2025-06-02 10:06:38'),(49,7,49,4,'Good display and ergonomic design.','2025-06-02 10:06:38'),(52,2,1,4,NULL,'2025-06-28 10:33:21'),(53,3,1,5,'good for writing','2025-06-28 10:34:15');
/*!40000 ALTER TABLE `reviews` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `UserID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Phone` varchar(100) DEFAULT NULL,
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `PasswordHash` varchar(255) NOT NULL,
  `PasswordSalt` varchar(255) NOT NULL,
  PRIMARY KEY (`UserID`),
  UNIQUE KEY `Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'Alice Johnson','alice.johnson@example.com','555-123-4567','2025-06-02 09:36:33','+Ix1hyiZpG8j9SJ6Hp0Wt2/LAg0+atyfBLx1qL7bUZc17WJd3OvjZB3Yny8SkpxyKopsyMRNqRJiGxI082QWbw==','+bb0DrWzlUrLJfpJIQXclA=='),(2,'Bob Smith','bob.smith@example.com','555-234-5678','2025-06-02 09:36:33','8zErR4RnBdaCgUqHTBvR8LL/d7nIeUSY3rgyLLWHs5mgP0ivDUJQhhwxE3N3etlBB88uKohKPWDgnvwvStThHQ==','axihaFK4gYWYSUqsh1JyGA=='),(3,'Charlie Brown','charlie.brown@example.com',NULL,'2025-06-02 09:36:33','utj8YTtWTH3djqcytpiSEOiIKvS2bDxqnDgfOB8iMUHRZHnu2Z4h73HB35rU4b/ek6qGvJdz/HsBgeRR5+lqng==','YEPiiF9SOr+aYU+/lFkncA=='),(4,'Diana Prince','diana.prince@example.com','555-345-6789','2025-06-02 09:36:33','nla7mW8YRf7BvYXPiDhx1k2xkFwACNTMswYOMGbun8cM9MXSuI+0B/1rvlX+Zr4zeQX+9AzSUy9YEaswogTT+A==','T/K38vKvkmI89AP0+eQfYA=='),(5,'Ethan Hunt','ethan.hunt@example.com',NULL,'2025-06-02 09:36:33','fZtwKU1DKzr2i6fvXLvRT6ckQytnOCSesIy6TL0SO5NSzbAeWQk1wDoZJoJb9FijJWCn8TaZk4IbfXBjp1iiXw==','SqYSxFOzEx/fbxUcfLlIoQ=='),(6,'Fiona Gallagher','fiona.g@example.com','555-456-7890','2025-06-02 09:36:33','OPNMfGgtCeNyUu2Fg5F1Qr550Ska041SAgduCbmxSYKs3PvYFhNq1aIoxKprcoPAZ9Z2UBJbsV/jYk051I5Ddw==','8+9CewH7TjKeBEhkQ9/Czg=='),(7,'George Martin','george.martin@example.com','555-567-8901','2025-06-02 09:36:33','gQKQoa7TzlsKLgM5/e+IZozIcpUJaVwenzr3ty2NCei4kVdbqFgniZpBEdwW1ntDHU21kCea79b+u+wSZMETUw==','ol9NKseVIh95bR2Wx1mu9w=='),(8,'Hannah Wells','hannah.w@example.com',NULL,'2025-06-02 09:36:33','7IYagfxr9ZhylWCOs6keofJL8r2JZsumfBr9EI/x8YOkSaI2Ti4V8NLlvPRhgE8aMB2QEX9v4AP2ixtGotXgvA==','/9aH8PJnmznqON+Wh/6Fow=='),(9,'Ian Fleming','ian.fleming@example.com','555-678-9012','2025-06-02 09:36:33','+rrxFMdDwvZqeWNtU9wTWLsl54gIaecjVxk//l2GVU/UUY8q5IyHVjov5d+U3V5ei7Ptj0gTsh4HPQqDnpV/+w==','SAixlhCpYQd0RxKH5aWzjQ=='),(10,'Julia Roberts','julia.roberts@example.com','555-789-0123','2025-06-02 09:36:33','GktWI3YLQ91YqqJxIFNc5dN/gcQoGWlYW0Ly6mL+l0AFvoZK41RrNHlg3vojZW+lFm+XFQj7xLf2uWtlv+pQYQ==','Cdy+5T0xbd+MDVKQvsikPA=='),(23,'random random','random@gmail.com','898977987','2025-06-26 18:11:27','FiZZYPRaxaX6Tqy0cBXZJprami9FwZ5JnaOuEOs3pTT/Hh0mQVFKhIqjkRj5QsGb0zvQPgetB+4vL1UINOfwVQ==','tskhyXaXyRGRFVTKQBKLTA=='),(33,'oooooooooooooooooooooooooooooooooooooooooooooooooo','oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo@gamil.com',NULL,'2025-07-02 10:03:34','nbfVGJ4r4p5YR4tlspE2/lpmp168EsZ/rAuYEuKdM6/6HddYkGZ1sTV08l39dRKc7/E6gHMnxYtn6MsHxl9MxQ==','UUknV9iBff6rGFs+a9hJqg=='),(36,'Test','test@gmail.com','46481648','2025-07-02 16:51:02','vBBIaQbXEJcN1/8t4+htrOEAztLLeuRW7CLX/FiWSB5W/HtHG0e+xp+Jc6Fwjuj4220p+nnAVsB7O9One656+g==','FccLMoSpBKpRkWRPG2d62Q==');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-07-05 19:24:57
