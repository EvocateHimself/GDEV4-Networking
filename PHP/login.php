<?php
// Connect to database
include("connect.inc");

// Variables
$loginUser = $_POST["loginUser"];
$loginPass = $_POST["loginPass"];

$query = "SELECT id, password FROM users WHERE username = '$loginUser' LIMIT 1";
$result = mysqli_query($db, $query) or die("Error querying database.");

// Check if database has users
if (mysqli_num_rows($result) > 0) { 
	// Output data of each row
	while ($row = mysqli_fetch_assoc($result)) {
		// Check if username is certain amount of characters
		if (strlen($loginUser) >= 4 && strlen($loginUser) <= 10) {
			// If password submitted matches the password in the database
			// if ($row["password"] == $loginPass) {
			$salt = "328yurfhu42fhurg7df";

			if (password_verify($loginPass.$salt, $row["password"])) {
				// echo "<color=lime>Login successful!</color>";

				echo $row["id"];

				//echo json_encode(array('result' => session_id())); // Output session id as result
			} else {
				echo "<color=red>Wrong credentials.</color>";
			}
		} else {
			echo "<color=blue>Username does not exist.</color>";
		}
	}
} else {
    echo "<color=blue>Username does not exist.</color>";
}

//Step 4
mysqli_close($db);

?>