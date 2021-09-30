<?php
// Connect to database
include("connect.inc");
?>

<html>
 <head>
 </head>
 <body style="text-align: center;">

<?php

$id = $_GET['id'];
$username = $_GET['username'];
$password = $_GET['password'];

// Check if id is int, and if username and password do not contain spaces
if (filter_var($id, FILTER_VALIDATE_INT) && strpos($username, ' ') === false && strpos($password, ' ') === false) { 

	$query = "SELECT * FROM users WHERE id = '$id' AND username = '$username' AND password = '$password' LIMIT 1";
	$result = mysqli_query($db, $query) or die("Error querying database.");

	if (mysqli_num_rows($result) > 0) {
		while ($row = mysqli_fetch_array($result, MYSQLI_ASSOC)) { // Only show associative keys, no numeric keys 
			?>
			<h1>Edit your account</h1>
			<p>You are logged in as <strong><?php echo $_GET['username'];?></strong></p><br>
			<form action="" method="post">
				<label for="fname">Username:</label><br><br>
				<input type="text" minlength="4" maxlength="10" name="inputUsername" placeholder="<?php echo $username?>"><br><br>
				<button type="submit" name="update-username">Change username</button><br><br>
				<label for="fname">Password:</label><br><br>
				<input type="password" minlength="4" maxlength="9" name="inputPassword" placeholder="<?php echo str_repeat("*", strlen($password))?>"><br><br>
				<input type="password" minlength="4" maxlength="9" name="inputPasswordRepeat" placeholder="Repeat Password..."><br><br>
				<button type="submit" name="update-password">Change password</button><br>
				
			</form>
			<?php 
			//echo json_encode($row);

			// When clicked on change username
		    if($_SERVER['REQUEST_METHOD'] == "POST" && isset($_POST['update-username'])) {

		    	$inputUsername = $_POST['inputUsername'];
		    	$getUsernameQuery = "SELECT username FROM users WHERE username = '$inputUsername' LIMIT 1";
				$usernameResult = mysqli_query($db, $getUsernameQuery) or die("Error querying database.");

				// Check if given input is not empty
				if (strlen($_POST['inputUsername']) >= 4 && strlen($_POST['inputUsername']) <= 10) {
					// Check if database has user with username
					if (mysqli_num_rows($usernameResult) > 0) { 
						// If username is the same as current username
						if ($inputUsername == $username) {
							echo "<span style='color: red;'>Username is the same as your current username!</span>";
						} else {
							// If username is claimed by someone else
							echo "<span style='color: red;'>Username already taken by someone else!</span>";
						}
					} else {
						// Update in database
						$updateUsernameQuery = "UPDATE users SET username = '$inputUsername' WHERE id = $id";
						$updateUsernameResult = mysqli_query($db, $updateUsernameQuery) or die("Error querying database.");
						echo("<script>alert('Username changed successfully!')</script>");
						echo("<script>window.location = 'edit_profile.php?id=$id&username=$inputUsername&password=$password';</script>");
						
					}
				} else {
					// If given input is empty
					echo "<span style='color: red;'>No username given!</span>";
				}
		    }

		    // When clicked on change password
		    if($_SERVER['REQUEST_METHOD'] == "POST" && isset($_POST['update-password'])) {

		    	$inputPassword = $_POST['inputPassword'];
		    	$inputPasswordRepeat = $_POST['inputPasswordRepeat'];
		    	
				// Check if given input is not empty
				if (strlen($_POST['inputPassword']) >= 4) {
					// If passwords match
					if ($inputPassword == $inputPasswordRepeat) {

						// Salt and hash password
						$salt = "328yurfhu42fhurg7df";
		    			$hashedPass = password_hash($inputPassword.$salt, PASSWORD_DEFAULT);

						// If password is not the same as the current password
						if (!password_verify($hashedPass, $password)) {		
							// Update in database
							$updatePasswordQuery = "UPDATE users SET password = '$hashedPass' WHERE id = $id";
							$updatePasswordResult = mysqli_query($db, $updatePasswordQuery) or die("Error querying database.");
							echo("<script>alert('Password changed successfully!')</script>");
							echo("<script>window.location = 'edit_profile.php?id=$id&username=$username&password=$hashedPass';</script>");
							//echo "<span style='color: green;'>Password changed successfully</span>";
						} else {
							echo "<span style='color: red;'>Password is the same as your current password!</span>";
						}
					} else {
						echo "<span style='color: red;'>Passwords don't match!</span>";
					}

				} else {
					// If given input is empty
					echo "<span style='color: red;'>No password given!</span>";
				}
		    }
		}
	} else {
		?>
		<h1>Access denied 2</h1>
		<p>You do not have permissions to access this page.</p>
		<?php 
	    //echo json_encode(array('id' => 0)); // Output 0 as result
	}
} else {
	?>
	<h1>Access denied</h1>
	<p>You do not have permissions to access this page.</p>
	<?php 
	//echo json_encode(array('id' => 0)); // Output 0 as result
}

//Step 4
mysqli_close($db);

?>

</body>
</html>