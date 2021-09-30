<?php
// Connect to database
include("connect.inc");
?>

<html>
 <head>
  <style>
	table, th, td {
	  border: 1px solid black;
	  text-align: center;
	  width: 15%;
	  height: 30px;
	}

	.games-table {
		width: 30%;
	}

	.vs {
		background-color: #000000;
		color: #ffffff;
		width: 10% !important;
	}

	.winner {
		background-color: #ccff33;
	}

	.loser {
		background-color: #ff6633;
	}

	.draw {
		background-color: #bbbbbb;
	}

	table {
	  margin-left: auto;
	  margin-right: auto;
	}

  </style>
 </head>
 <body style="text-align: center;">
 <h1>Highscores</h1>
 <h3>Top 5 Players</h3>
<?php

//Step2
$scoreQuery = "SELECT * FROM users ORDER BY score DESC LIMIT 5";
mysqli_query($db, $scoreQuery) or die('Error querying database.');

$gamesQuery = "SELECT * FROM scores ORDER BY time_stamp DESC LIMIT 10";
mysqli_query($db, $gamesQuery) or die('Error querying database.');

//Step3
$scoreResult = mysqli_query($db, $scoreQuery);
$gamesResult = mysqli_query($db, $gamesQuery);

?>

<table>
 <tr>
  <th>Name</th>
  <th>Score</th>
 </tr>

<?php

while ($row = mysqli_fetch_array($scoreResult)) {
 echo "<tr>";
 echo "<td>".$row['username']."</td>";
 echo "<td>".$row['score']."</td>";
 echo "</tr>";
}

?>

</table>
<br>
<br>
<h3>Last Played Games</h3>

<table class="games-table">
 <tr>
  <th>Player 1</th>
  <th>Score</th>
  <th class="vs">VS</th>
  <th>Player 2</th>
  <th>Score</th>
  <th>Date</th>
 </tr>
<?php

// Give winners green color, losers red color, draw grey color
while ($row = mysqli_fetch_array($gamesResult)) {
 if ($row['score_player1'] > $row['score_player2']) {
 	echo "<tr>";
 	echo "<td class='winner'>".$row['username_player1']."</td>";
 	echo "<td class='winner'>".$row['score_player1']."</td>";
 	echo "<td class='vs'>VS</td>";
 	echo "<td class='loser'>".$row['username_player2']."</td>";
 	echo "<td class='loser'>".$row['score_player2']."</td>";
 	echo "<td>".$row['time_stamp']."</td>";
 	echo "</tr>";
 } else if ($row['score_player1'] < $row['score_player2']) {
 	echo "<tr>";
 	echo "<td class='loser'>".$row['username_player1']."</td>";
 	echo "<td class='loser'>".$row['score_player1']."</td>";
 	echo "<td class='vs'>VS</td>";
 	echo "<td class='winner'>".$row['username_player2']."</td>";
 	echo "<td class='winner'>".$row['score_player2']."</td>";
 	echo "<td>".$row['time_stamp']."</td>";
 	echo "</tr>";
 } else {
 	echo "<tr>";
 	echo "<td class='draw'>".$row['username_player1']."</td>";
 	echo "<td class='draw'>".$row['score_player1']."</td>";
 	echo "<td class='vs'>VS</td>";
 	echo "<td class='draw'>".$row['username_player2']."</td>";
 	echo "<td class='draw'>".$row['score_player2']."</td>";
 	echo "<td>".$row['time_stamp']."</td>";
 	echo "</tr>";
 }

}

?>

</table>


<?php

//Step 4
mysqli_close($db);

?>

</body>
</html>