<?php

	// Include sql-server data
	require_once "connect.php";
	
	// Get account data by POST method from C# App
	$login = $_POST['login'];
	
	try
	{
		// Try to connect with database
		$connection = new mysqli($host, $db_user, $db_password, $db_name);
		// If failed, throw exception
		if ($connection->connect_errno!=0)
				throw new Exception(mysqli_connect_errno());
		else
		{
			// Connected with database, try to find user
			$result = $connection->query("SELECT * FROM users WHERE login = '$login'");
			// If failed to send query, throw exception
			if (!$result) throw new Exception($connection->error);
			
			// Fetch data
          	$row = mysqli_fetch_assoc($result);
			
			// Output balance and id
			echo $row['currency']."/".$row['id'];
		}
	}
	catch (Exception $ex)
	{
		echo "Not Found";
	}

?>