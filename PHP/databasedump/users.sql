-- phpMyAdmin SQL Dump
-- version 4.9.7
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Gegenereerd op: 30 sep 2021 om 04:21
-- Serverversie: 10.4.17-MariaDB-cll-lve
-- PHP-versie: 7.0.33

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `kevinwy331_gdev4`
--

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `users`
--

CREATE TABLE `users` (
  `id` int(4) NOT NULL,
  `username` varchar(10) CHARACTER SET utf8 NOT NULL,
  `email` varchar(255) CHARACTER SET utf8 NOT NULL,
  `password` varchar(255) CHARACTER SET utf8 NOT NULL,
  `score` int(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Gegevens worden geëxporteerd voor tabel `users`
--

INSERT INTO `users` (`id`, `username`, `email`, `password`, `score`) VALUES
(87, 'ijsthee', '', '$2y$10$PC0cC50UIpk4HgE7Tugo0u8BmOZTJp5bjO4PiVc93r4s/hxNZ69vq', 0),
(88, 'yoda', '', '$2y$10$lObOdo/FH84LY4rLMh4lMeptwRTAsR7/by/2ZmDEdOCJfhIykOjtK', 8),
(89, 'boomer', '', '$2y$10$TvM.7YqgIjycNDVEX01zMuknz2Jnfg.LT2tRVlsO1zdKE9rPGn4WG', 5),
(90, 'test', '', '$2y$10$jt4ZgNpLNuBfsbhE2Bf8S.Os9snv8KSuXAd9QkYszhoSXRrXWgDLm', 12),
(91, 'kees', '', '$2y$10$Wude9kjNyBnx7wkVp0LFYuJOkoc5vERxP1s0P3lKuNSeiG/vBTQY2', 5);

--
-- Indexen voor geëxporteerde tabellen
--

--
-- Indexen voor tabel `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT voor geëxporteerde tabellen
--

--
-- AUTO_INCREMENT voor een tabel `users`
--
ALTER TABLE `users`
  MODIFY `id` int(4) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=92;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
