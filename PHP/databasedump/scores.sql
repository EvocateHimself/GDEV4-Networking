-- phpMyAdmin SQL Dump
-- version 4.9.7
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Gegenereerd op: 30 sep 2021 om 04:20
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
-- Tabelstructuur voor tabel `scores`
--

CREATE TABLE `scores` (
  `id` int(4) NOT NULL,
  `server_id` int(255) NOT NULL,
  `username_player1` varchar(10) NOT NULL,
  `score_player1` int(255) NOT NULL,
  `username_player2` varchar(10) NOT NULL,
  `score_player2` int(255) NOT NULL,
  `time_stamp` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Gegevens worden geëxporteerd voor tabel `scores`
--

INSERT INTO `scores` (`id`, `server_id`, `username_player1`, `score_player1`, `username_player2`, `score_player2`, `time_stamp`) VALUES
(30, 0, 'test', 1, 'yoda', 1, '2021-09-28 23:56:45'),
(31, 0, 'test', 1, 'yoda', 2, '2021-09-28 23:58:04'),
(32, 0, 'test', 3, 'yoda', 1, '2021-09-29 01:03:39'),
(33, 0, 'boomer', 1, 'kees', 2, '2021-09-29 01:16:54'),
(34, 0, 'test', 2, 'boomer', 1, '2021-09-29 19:36:08'),
(35, 0, 'boomer', 1, 'test', 1, '2021-09-29 19:39:03'),
(36, 0, 'boomer', 2, 'test', 1, '2021-09-29 20:01:37'),
(37, 0, 'kees', 2, 'test', 1, '2021-09-29 21:11:23'),
(38, 0, 'test', 2, 'kees', 1, '2021-09-30 02:09:22');

--
-- Indexen voor geëxporteerde tabellen
--

--
-- Indexen voor tabel `scores`
--
ALTER TABLE `scores`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT voor geëxporteerde tabellen
--

--
-- AUTO_INCREMENT voor een tabel `scores`
--
ALTER TABLE `scores`
  MODIFY `id` int(4) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=39;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
