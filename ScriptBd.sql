-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 10-10-2025 a las 01:33:22
-- Versión del servidor: 10.4.28-MariaDB
-- Versión de PHP: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria1`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `idContrato` int(11) NOT NULL,
  `fechaInicio` date NOT NULL,
  `fechaFinOriginal` date NOT NULL,
  `fechaFinEfectiva` date DEFAULT NULL,
  `montoMensual` decimal(10,2) NOT NULL,
  `estado` tinyint(1) NOT NULL DEFAULT 1,
  `idInquilino` int(11) NOT NULL,
  `idInmueble` int(11) NOT NULL,
  `usuarioCreador` int(11) DEFAULT NULL,
  `usuarioFinalizador` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`idContrato`, `fechaInicio`, `fechaFinOriginal`, `fechaFinEfectiva`, `montoMensual`, `estado`, `idInquilino`, `idInmueble`, `usuarioCreador`, `usuarioFinalizador`) VALUES
(2, '2024-10-31', '2025-09-19', NULL, 50000.00, 1, 31, 5, NULL, NULL),
(6, '2025-09-04', '2025-09-27', NULL, 120000.00, 0, 1, 3, NULL, NULL),
(7, '2025-09-01', '2025-09-06', NULL, 21311.00, 0, 3, 3, NULL, NULL),
(8, '2025-09-04', '2025-09-27', '2025-10-23', 120000.00, 0, 1, 2, NULL, 2),
(9, '2025-09-04', '2025-09-27', NULL, 120000.00, 1, 1, 2, NULL, NULL),
(10, '2025-09-04', '2025-09-27', NULL, 120000.00, 1, 4, 5, NULL, NULL),
(11, '2025-09-04', '2025-09-27', NULL, 120000.00, 0, 3, 2, NULL, NULL),
(12, '2025-10-01', '2025-10-18', NULL, 50000.00, 1, 3, 3, NULL, NULL),
(13, '2025-10-03', '2025-10-11', NULL, 120000.00, 1, 31, 2, 2, NULL),
(14, '2025-10-09', '2025-10-11', NULL, 120000.00, 1, 31, 8, 2, NULL),
(15, '2025-10-20', '0001-01-01', NULL, 99999999.99, 1, 13, 5, 2, NULL),
(16, '2025-10-15', '0001-01-01', NULL, 99999999.99, 1, 7, 8, 2, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `imagen`
--

CREATE TABLE `imagen` (
  `idImagen` int(11) NOT NULL,
  `idInmueble` int(11) NOT NULL,
  `url` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `imagen`
--

INSERT INTO `imagen` (`idImagen`, `idInmueble`, `url`) VALUES
(21, 2, '/Uploads/Inmuebles/2/614cf291-f2af-42e1-8588-020beb3961e3.jpg'),
(22, 2, '/Uploads/Inmuebles/2/28fe1d89-1d52-462a-b500-ca8335d3d01e.jpg'),
(23, 2, '/Uploads/Inmuebles/2/1bae9a18-f5e5-4e37-88d5-c6efab5191eb.jpg'),
(24, 3, '/Uploads/Inmuebles/3/72d764e0-a52f-4f84-9505-ca18d5076b76.jpg'),
(25, 3, '/Uploads/Inmuebles/3/49832d5f-ee96-4d9f-89ca-6a8ecc315008.jpg'),
(26, 3, '/Uploads/Inmuebles/3/7051be59-79d0-4a03-8623-bbd1e71953be.jpg'),
(27, 5, '/Uploads/Inmuebles/5/a2ded9ac-d54c-456a-8862-f697bd950ae1.jpg'),
(28, 5, '/Uploads/Inmuebles/5/b378ff92-5cfb-457f-b726-dea23e5826fe.jpg'),
(29, 5, '/Uploads/Inmuebles/5/193980d8-46ca-4026-a868-cb507699ef0b.jpg');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `idInmueble` int(11) NOT NULL,
  `direccion` varchar(200) NOT NULL,
  `uso` enum('Residencial','Comercial') NOT NULL,
  `ambientes` int(11) NOT NULL,
  `precio` decimal(10,2) NOT NULL,
  `latitud` decimal(30,0) NOT NULL,
  `longitud` decimal(30,0) NOT NULL,
  `estado` enum('Disponible','Alquilado','Vendido','Suspendido') NOT NULL DEFAULT 'Disponible',
  `tipo` enum('Local','Deposito','Casa','Departamento') NOT NULL,
  `portada` varchar(255) DEFAULT NULL,
  `idPropietario` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`idInmueble`, `direccion`, `uso`, `ambientes`, `precio`, `latitud`, `longitud`, `estado`, `tipo`, `portada`, `idPropietario`) VALUES
(2, 'aristobulo 58', 'Comercial', 2, 334.45, 56, 55, 'Disponible', 'Casa', NULL, 3),
(3, 'San Martin 205', 'Residencial', 4, 2312314.00, 678, 1231, 'Alquilado', 'Deposito', '/Uploads/Inmuebles\\portada_3.jpg', 3),
(5, 'Pinamar 120', 'Comercial', 2, 5700000.00, 444, 1231, 'Disponible', 'Deposito', '/Uploads/Inmuebles\\portada_5.jpg', 1),
(6, 'Las Heras 890', 'Residencial', 2, 5700000.00, 444, 1231, 'Suspendido', 'Deposito', '/Uploads/Inmuebles\\portada_6.webp', 6),
(7, 'San Martin 500', 'Comercial', 5, 80000000.00, 677, 1112, 'Vendido', 'Casa', '/Uploads/Inmuebles\\portada_7.jpg', 6),
(8, 'calle falsa 444', 'Comercial', 2, 99999999.99, 45, 666, 'Disponible', 'Local', NULL, 6),
(9, 'Rivadavia 1200', 'Residencial', 3, 7500000.00, 334, 554, 'Disponible', 'Departamento', NULL, 6),
(10, 'Belgrano 450', 'Comercial', 2, 12000000.00, 335, 552, 'Disponible', 'Local', NULL, 7),
(11, 'Italia 987', 'Residencial', 4, 9800000.00, 337, 556, 'Disponible', 'Casa', NULL, 8),
(12, 'Mitre 222', 'Comercial', 1, 4500000.00, 338, 559, 'Disponible', 'Local', NULL, 9),
(13, '9 de Julio 765', 'Residencial', 2, 6800000.00, 339, 560, 'Disponible', 'Departamento', NULL, 10),
(14, 'Las Flores 112', 'Residencial', 5, 15000000.00, 340, 561, 'Disponible', 'Casa', NULL, 11),
(15, 'Libertad 88', 'Comercial', 3, 8900000.00, 341, 562, 'Disponible', 'Deposito', NULL, 12),
(16, 'San Luis 301', 'Residencial', 4, 9700000.00, 342, 563, 'Disponible', 'Casa', NULL, 13),
(17, 'Buenos Aires 44', 'Comercial', 2, 8100000.00, 343, 564, 'Disponible', 'Local', NULL, 14),
(18, 'España 199', 'Residencial', 3, 7200000.00, 344, 565, 'Disponible', 'Departamento', NULL, 15);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `idInquilino` int(11) NOT NULL,
  `dni` varchar(20) NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `apellido` varchar(45) NOT NULL,
  `telefono` varchar(45) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `estado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`idInquilino`, `dni`, `nombre`, `apellido`, `telefono`, `email`, `estado`) VALUES
(1, '56783321', 'Pedro', 'Perez', '14141414', 'jose@gmail.com.ar', 0),
(3, '45678233', 'agustin', 'Lopez', '2665434567', 'aguuss@hotlmaim.com', 0),
(4, '222313113', 'Joselito', 'Gutierritos', '123123125', 'josje@gmail.com', 1),
(5, '11111111', 'Ana', 'García', '111111111', 'ana.garcia@email.com', 1),
(6, '23222222', 'Luis', 'Martínez', '262222222', 'luis.martinez@email.com', 1),
(7, '33333333', 'Sofía', 'Rodríguez', '313333333', 'sofia.rodriguez@email.com', 1),
(8, '44444444', 'Carlos', 'López', '444444444', 'carlos.lopez@email.com', 1),
(9, '55555555', 'Elena', 'Sánchez', '578555555', 'elena.sanchez@email.com', 1),
(10, '66666666', 'Javier', 'Fernández', '6676866666', 'javier.fernandez@email.com', 0),
(11, '77777777', 'Laura', 'Gómez', '777778777', 'laura.gomez@email.com', 1),
(12, '88888888', 'Diego', 'Pérez', '888898888', 'diego.perez@email.com', 1),
(13, '99999999', 'María', 'Díaz', '999979999', 'maria.diaz@email.com', 1),
(14, '10101010', 'José', 'Ruiz', '101010101', 'jose.ruiz@email.com', 1),
(15, '12121212', 'Marta', 'Herrera', '121212121', 'marta.herrera@email.com', 1),
(16, '13131313', 'Pablo', 'Jiménez', '131313131', 'pablo.jimenez@email.com', 1),
(17, '14141414', 'Sara', 'Vázquez', '141414141', 'sara.vazquez@email.com', 1),
(18, '15151515', 'Andrés', 'Castro', '151515151', 'andres.castro@email.com', 1),
(19, '16161616', 'Clara', 'Moreno', '161616161', 'clara.moreno@email.com', 1),
(20, '17171717', 'Sergio', 'Ortega', '171717171', 'sergio.ortega@email.com', 1),
(21, '18181818', 'Isabel', 'Ramírez', '181818181', 'isabel.ramirez@email.com', 1),
(22, '19191919', 'Fernando', 'Serrano', '191919191', 'fernando.serrano@email.com', 1),
(23, '20202020', 'Alicia', 'Gil', '202020202', 'alicia.gil@email.com', 1),
(24, '21212121', 'Roberto', 'Navarro', '212121212', 'roberto.navarro@email.com', 1),
(25, '22222222', 'Cristina', 'Delgado', '222292222', 'cristina.delgado@email.com', 1),
(26, '23232323', 'Manuel', 'Ibarra', '232323232', 'manuel.ibarra@email.com', 1),
(27, '24242424', 'Paula', 'Cortés', '242424242', 'paula.cortes@email.com', 1),
(28, '25252525', 'Rubén', 'Medina', '252525252', 'ruben.medina@email.com', 1),
(29, '26262626', 'Silvia', 'Blanco', '262626262', 'silvia.blanco@email.com', 1),
(31, '234245112', 'agustin', 'perez', '1231232', 'juan2chi@hotlmaim.com', 1),
(32, '55555558', 'Martina', 'Gomez', '123123123', 'aguas@hotlmaim.com', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `idPago` int(11) NOT NULL,
  `numPago` int(11) NOT NULL,
  `fechaPago` date NOT NULL,
  `importe` decimal(10,2) NOT NULL,
  `concepto` varchar(200) DEFAULT NULL,
  `estado` tinyint(4) NOT NULL,
  `idContrato` int(11) NOT NULL,
  `usuarioCreador` int(11) NOT NULL,
  `usuarioAnulador` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`idPago`, `numPago`, `fechaPago`, `importe`, `concepto`, `estado`, `idContrato`, `usuarioCreador`, `usuarioAnulador`) VALUES
(1, 1, '2025-09-10', 50000.00, 'Pago mes septiembre', 0, 2, 4, NULL),
(2, 2, '2025-10-10', 50000.00, 'Pago mes octubre', 1, 2, 4, NULL),
(3, 1, '2025-09-05', 120000.00, 'Pago inicial', 0, 8, 5, NULL),
(4, 2, '2025-10-05', 120000.00, 'Pago segundo mes', 0, 8, 5, NULL),
(5, 1, '2025-09-06', 120000.00, 'Pago único', 0, 9, 5, NULL),
(6, 1, '2025-09-07', 21311.00, 'Pago único', 0, 7, 4, NULL),
(7, 1, '2025-09-04', 120000.00, 'Pago inicial', 0, 6, 2, 4),
(8, 2, '2025-09-20', 120000.00, 'Pago septiembre', 0, 6, 2, NULL),
(9, 1, '2025-09-05', 120000.00, 'Pago septiembre', 0, 10, 4, NULL),
(10, 1, '2025-10-03', 50000.00, 'Pago mes octubre', 0, 12, 5, NULL),
(11, 1, '2025-09-10', 120000.00, 'Pago septiembre', 0, 11, 2, NULL),
(12, 1, '2025-09-15', 120000.00, 'Pago adelantado', 0, 9, 4, NULL),
(13, 2, '2025-10-15', 120000.00, 'septi', 1, 9, 4, NULL),
(14, 1, '2025-09-28', 120000.00, 'Pago único', 0, 11, 2, 5),
(15, 88, '2025-10-26', 90000000.00, 'Noviembre', 2, 8, 2, 2),
(16, 55555, '2025-06-18', 80000000.00, 'Noviembre', 2, 9, 2, 2),
(17, 333, '2025-10-02', 11111.00, 'Noviembre', 0, 7, 2, 2),
(18, 11, '2025-10-06', 13131313.00, 'Noviembre', 0, 9, 2, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `idPropietario` int(11) NOT NULL,
  `dni` varchar(20) NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `apellido` varchar(45) NOT NULL,
  `telefono` varchar(45) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `estado` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`idPropietario`, `dni`, `nombre`, `apellido`, `telefono`, `email`, `estado`) VALUES
(1, '32133664', 'martin', 'Herrera', '266427543655', 'agus@hotlmaim.com', 0),
(3, '423335565', 'Martina', 'Gomez', '4324525277', 'mar@Gmail.com', 0),
(4, '55555555', 'Martina', 'Gomez', '432452523', 'tammar@Gmail.com', 1),
(6, '54678978', 'Fabricio', 'Lucero', '2664456785', 'Fabricio12@gmail.com', 1),
(7, '12000001', 'Lucas', 'Pérez', '2664000001', 'lucas.perez@email.com', 1),
(8, '13000002', 'Valentina', 'Gómez', '2664000002', 'valentina.gomez@email.com', 1),
(9, '14000003', 'Joaquín', 'Rodríguez', '2664000003', 'joaquin.rodriguez@email.com', 1),
(10, '15000004', 'Camila', 'López', '2664000004', 'camila.lopez@email.com', 1),
(11, '16000005', 'Mateo', 'Fernández', '2664000005', 'mateo.fernandez@email.com', 1),
(12, '17000006', 'Martina', 'Sánchez', '2664000006', 'martina.sanchez@email.com', 1),
(13, '18000007', 'Benjamín', 'Romero', '2664000007', 'benjamin.romero@email.com', 1),
(14, '19000008', 'Isabella', 'Díaz', '2664000008', 'isabella.diaz@email.com', 1),
(15, '20000009', 'Tomás', 'Ruiz', '2664000009', 'tomas.ruiz@email.com', 1),
(16, '21000010', 'Catalina', 'Torres', '2664000010', 'catalina.torres@email.com', 1),
(17, '22000011', 'Santiago', 'Flores', '2664000011', 'santiago.flores@email.com', 1),
(18, '23000012', 'Mía', 'Acosta', '2664000012', 'mia.acosta@email.com', 1),
(19, '24000013', 'Thiago', 'Vega', '2664000013', 'thiago.vega@email.com', 1),
(20, '70000014', 'Emma', 'Molina', '2664000014', 'emma.molina@email.com', 1),
(21, '70000015', 'Valentino', 'Castro', '2664000015', 'valentino.castro@email.com', 1),
(22, '70000016', 'Sofía', 'Morales', '2664000016', 'sofia.morales@email.com', 1),
(23, '70000017', 'Julieta', 'Silva', '2664000017', 'julieta.silva@email.com', 1),
(24, '70000018', 'Franco', 'Ortega', '2664000018', 'franco.ortega@email.com', 1),
(25, '70000019', 'Renata', 'Navarro', '2664000019', 'renata.navarro@email.com', 1),
(26, '70000020', 'Bautista', 'Delgado', '2664000020', 'bautista.delgado@email.com', 1),
(27, '70000021', 'Luna', 'Ibarra', '2664000021', 'luna.ibarra@email.com', 1),
(28, '70000022', 'Lautaro', 'Méndez', '2664000022', 'lautaro.mendez@email.com', 1),
(29, '70000023', 'Josefina', 'Ramos', '2664000023', 'josefina.ramos@email.com', 1),
(30, '70000024', 'Ignacio', 'Campos', '2664000024', 'ignacio.campos@email.com', 1),
(31, '70000025', 'Victoria', 'Reyes', '2664000025', 'victoria.reyes@email.com', 1),
(32, '70000026', 'Gael', 'Herrera', '2664000026', 'gael.herrera@email.com', 1),
(33, '70000027', 'Olivia', 'Jiménez', '2664000027', 'olivia.jimenez@email.com', 1),
(34, '70000028', 'Simón', 'Luna', '2664000028', 'simon.luna@email.com', 1),
(35, '70000029', 'Emily', 'Cabrera', '2664000029', 'emily.cabrera@email.com', 1),
(36, '70000030', 'Emilio', 'Figueroa', '2664000030', 'emilio.figueroa@email.com', 1),
(37, '70000031', 'Lucía', 'Paredes', '2664000031', 'lucia.paredes@email.com', 1),
(38, '70000032', 'Felipe', 'Cardozo', '2664000032', 'felipe.cardozo@email.com', 1),
(39, '70000033', 'Antonella', 'Villalba', '2664000033', 'antonella.villalba@email.com', 0),
(40, '70000034', 'Máximo', 'Salazar', '2664000034', 'maximo.salazar@email.com', 1),
(41, '70000035', 'Abigail', 'Correa', '2664000035', 'abigail.correa@email.com', 1),
(42, '70000036', 'Aaron', 'Córdoba', '2664000036', 'aaron.cordoba@email.com', 1),
(43, '70000037', 'Zoe', 'Leiva', '2664000037', 'zoe.leiva@email.com', 1),
(44, '70000038', 'Juan', 'Roldán', '2664000038', 'juan.roldan@email.com', 1),
(45, '70000039', 'Mila', 'Palacios', '2664000039', 'mila.palacios@email.com', 1),
(46, '70000040', 'Dante', 'Peralta', '2664000040', 'dante.peralta@email.com', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `idUsuario` int(11) NOT NULL,
  `email` varchar(100) NOT NULL,
  `clave` varchar(255) NOT NULL,
  `rol` varchar(45) NOT NULL,
  `nombre` varchar(25) NOT NULL,
  `apellido` varchar(25) NOT NULL,
  `avatar` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`idUsuario`, `email`, `clave`, `rol`, `nombre`, `apellido`, `avatar`) VALUES
(2, 'Dios@gmail.com', 'GAKKw6Co5EiIGNiZC1OfQC6offL+e8CoEs3SX0LIrHA=', '0', 'Dios', 'a', '/Uploads\\avatar_2.jpg'),
(4, 'Usuario@hotmail', 'NrRPI48ywkKJIZ05zWza3tHgPw2dUPqxldi9UxLvDuY=', '1', 'goku', '2334', '/Uploads\\avatar_4.jpg'),
(5, 'adasdas@gmail.com', 'NrRPI48ywkKJIZ05zWza3tHgPw2dUPqxldi9UxLvDuY=', '1', 'agustin5', 'Herrera', '/Uploads\\avatar_5.jpeg'),
(6, 'este@gmail.com', 'GAKKw6Co5EiIGNiZC1OfQC6offL+e8CoEs3SX0LIrHA=', '0', 'agustin', 'Herrera2', NULL);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`idContrato`),
  ADD KEY `fk_contratos_inquilinos1_idx` (`idInquilino`),
  ADD KEY `fk_contratos_inmuebles1_idx` (`idInmueble`),
  ADD KEY `fk_contratos_usuarios1_idx` (`usuarioCreador`),
  ADD KEY `fk_contratos_usuarios2_idx` (`usuarioFinalizador`);

--
-- Indices de la tabla `imagen`
--
ALTER TABLE `imagen`
  ADD PRIMARY KEY (`idImagen`),
  ADD KEY `fk_imagen_inmueble1_idx` (`idInmueble`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`idInmueble`),
  ADD KEY `fk_inmuebles_propietarios_idx` (`idPropietario`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`idInquilino`),
  ADD UNIQUE KEY `dni_UNIQUE` (`dni`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`idPago`),
  ADD KEY `fk_pagos_contratos1_idx` (`idContrato`),
  ADD KEY `fk_pagos_usuarios1_idx` (`usuarioCreador`),
  ADD KEY `fk_pagos_usuarios2_idx` (`usuarioAnulador`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`idPropietario`),
  ADD UNIQUE KEY `dni_UNIQUE` (`dni`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`idUsuario`),
  ADD UNIQUE KEY `email_UNIQUE` (`email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `idContrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT de la tabla `imagen`
--
ALTER TABLE `imagen`
  MODIFY `idImagen` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=30;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `idInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `idInquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=33;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `idPago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `idPropietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=47;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `idUsuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `fk_contratos_inmuebles1` FOREIGN KEY (`idInmueble`) REFERENCES `inmueble` (`idInmueble`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_contratos_inquilinos1` FOREIGN KEY (`idInquilino`) REFERENCES `inquilino` (`idInquilino`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_contratos_usuarios1` FOREIGN KEY (`usuarioCreador`) REFERENCES `usuario` (`idUsuario`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_contratos_usuarios2` FOREIGN KEY (`usuarioFinalizador`) REFERENCES `usuario` (`idUsuario`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Filtros para la tabla `imagen`
--
ALTER TABLE `imagen`
  ADD CONSTRAINT `fk_imagen_inmueble1` FOREIGN KEY (`idInmueble`) REFERENCES `inmueble` (`idInmueble`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `fk_inmuebles_propietarios` FOREIGN KEY (`idPropietario`) REFERENCES `propietario` (`idPropietario`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `fk_pagos_contratos1` FOREIGN KEY (`idContrato`) REFERENCES `contrato` (`idContrato`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_pagos_usuarios1` FOREIGN KEY (`usuarioCreador`) REFERENCES `usuario` (`idUsuario`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_pagos_usuarios2` FOREIGN KEY (`usuarioAnulador`) REFERENCES `usuario` (`idUsuario`) ON DELETE NO ACTION ON UPDATE NO ACTION;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
