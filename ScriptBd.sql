-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 08-09-2025 a las 08:38:59
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
  `estado` varchar(45) NOT NULL,
  `idInquilino` int(11) NOT NULL,
  `idInmueble` int(11) NOT NULL,
  `usuarioCreador` int(11) NOT NULL,
  `usuarioFinalizador` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

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
  `idPropietario` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

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
  `estado` varchar(45) NOT NULL,
  `idContrato` int(11) NOT NULL,
  `usuarioCreador` int(11) NOT NULL,
  `usuarioAnulador` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

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

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `idUsuario` int(11) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password` varchar(255) NOT NULL,
  `rol` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

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
  MODIFY `idContrato` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `idInmueble` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `idInquilino` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `idPago` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `idPropietario` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `idUsuario` int(11) NOT NULL AUTO_INCREMENT;

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
