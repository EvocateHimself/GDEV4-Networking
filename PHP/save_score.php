<?php
// Connect to database
include("connect.inc");

// Variables
$userID = $_POST["userID"];
$userScore = $_POST["userScore"];

$query = "SELECT id, score FROM users WHERE id = '$userID' LIMIT 1";
$result = mysqli_query($db, $query) or die("Error querying database.");

// Check if database has users
if (mysqli_num_rows($result) > 0) { 

	// Update in database
	$updateScoreQuery = "UPDATE users SET score = '$userScore' WHERE id = $userID";
	$updateScoreResult = mysqli_query($db, $updateScoreQuery) or die("Error querying database.");

    if (mysqli_query($db, $updateScoreQuery)) {
	  	echo $row["score"];
	} else {
	  echo "Error: " . $updateScoreQuery . "<br>" . mysqli_error($db);
	}
}

//Step 4
mysqli_close($db);

?>