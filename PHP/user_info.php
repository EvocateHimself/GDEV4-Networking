<?php
// Connect to database
include("connect.inc");

// Variables
$userID = $_POST["userID"];

$query = "SELECT id, username, password, score FROM users WHERE id = '$userID'";
$result = mysqli_query($db, $query) or die("Error querying database.");

// Check if database has users
if (mysqli_num_rows($result) > 0) { 

	while ($row = mysqli_fetch_array($result, MYSQLI_ASSOC)) { // Only show associative keys, no numeric keys
	 //echo $row['id'] . ' ' . $row['username'] . ': ' . $row['email'] . ' ' . $row['password'] . ' ' . $row['points'] .'<br/>';
	 echo json_encode($row);
	}
} else {
    echo "<color=red>0 results.</color>";
}

//Step 4
mysqli_close($db);

?>