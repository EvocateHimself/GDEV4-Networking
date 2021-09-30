<?php
// Connect to database
include("connect.inc");

// Variables
$registerUser = $_POST["registerUser"];
$registerPass = $_POST["registerPass"];
$registerPassRepeat = $_POST["registerPassRepeat"];

$query = "SELECT id, username FROM users WHERE username = '$registerUser' LIMIT 1";
$result = mysqli_query($db, $query) or die("Error querying database.");

// Check if database has users
if (mysqli_num_rows($result) > 0) { 
	// Tell user that name is already taken
	echo "<color=red>Username is already taken.</color>";

} else {
	// Check if username is certain amount of characters
	if (strlen($registerUser) >= 4 && strlen($registerUser) <= 10) {
		// Check is password is certain amount of characters
		if (strlen($registerPass) >= 4) {
			// Check if passwords match and username is not empty
			if ($registerPass == $registerPassRepeat) {
				// echo "<color=blue>Creating user...</color>";

				// Salt and hash password
				$salt = "328yurfhu42fhurg7df";
				$hashedPass = password_hash($registerPass.$salt, PASSWORD_DEFAULT);

			    // Insert the username and password into the database
			    $createUserQuery = "INSERT INTO users (username, password, score) VALUES ('" . $registerUser . "', '" . $hashedPass . "', 0)";

			    if (mysqli_query($db, $createUserQuery)) {
				  	echo "<color=green>New user created successfully. Go back to login.</color>";
				  	echo $row["id"];
				} else {
				  echo "Error: " . $createUserQuery . "<br>" . mysqli_error($db);
				}
			} else {
				echo "<color=red>Passwords don't match.</color>";
			}
		} else {
			echo "<color=red>Password has to be at least 4 characters long.</color>";
		}
	} else {
		echo "<color=red>Username has to be at least 4 and max. 10 characters long.</color>";
	}
}

//Step 4
mysqli_close($db);

?>