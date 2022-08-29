<?php

	// Include sql-server data
	require_once "connect.php";
	
	// Get account data by POST method from C# App
	$id = $_POST['id'];
	$balance = $_POST['balance'];
	
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
			$result = $connection->query("UPDATE users SET currency = '$balance' WHERE id = '$id'");
			// If failed to send query, throw exception
			if (!$result) throw new Exception($connection->error);
			echo "Succeed";
		}
	}
	catch (Exception $ex)
	{
		echo "Not found";
	}

?>