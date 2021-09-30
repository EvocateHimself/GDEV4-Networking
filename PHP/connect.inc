<?php
$db_host = 'localhost';
$db_user = 'kevinwy331_gdev4';
$db_pass = 'hku2021';
$db_name = 'kevinwy331_gdev4';

$db = mysqli_connect('localhost', 'kevinwy331_gdev4', 'hku2021', 'kevinwy331_gdev4');

// Check connection
if (mysqli_connect_errno()) {
    echo "Error connecting to MySQL server."; // Connection failed
    exit();
}

// Throw error in case something goes wrong
function showerror($error,$errornr) {
die("Error (" . $errornr . ") " . $error);
}
?>