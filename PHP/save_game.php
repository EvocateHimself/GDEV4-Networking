<?php
// Connect to database
include("connect.inc");

// Variables
$username_p1 = $_POST["username_player1"];
$username_p2 = $_POST["username_player2"];
$score_p1 = $_POST["score_player1"];
$score_p2 = $_POST["score_player2"];

$query = "SELECT username_player1, score_player1, username_player2, score_player2, time_stamp FROM scores";
$result = mysqli_query($db, $query) or die("Error querying database.");
	
// Check if points are an int value
if (filter_var($score_p1, FILTER_VALIDATE_INT) && filter_var($score_p2, FILTER_VALIDATE_INT)) {  
	
	// Update in database
	$scores_query = "INSERT INTO scores (username_player1, score_player1, username_player2, score_player2) VALUES ('$username_p1', '$score_p1', '$username_p2', '$score_p2')";
	$scores_result = mysqli_query($db, $scores_query) or die("Error querying database.");
}

//Step 4
mysqli_close($db);

?>