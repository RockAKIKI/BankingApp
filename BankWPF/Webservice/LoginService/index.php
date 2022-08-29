<?php

	// Include sql-server data
	require_once "connect.php";
	
	// Get account data by GET method from C# App
	$login = $_POST['login'];
	$password = $_POST['password'];
	
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
			
			// Check how many users found
			$howManyUsers = $result->num_rows;
			if ($howManyUsers > 0)
			{
				// Fetch data from query
				$readout = $result->fetch_assoc();
				// Check if password is correct
				if (password_verify($password, $readout['password']))
				{
					// Output account data
					echo $readout['id'].'/'.$readout['login'].'/'.$readout['currency'];
				}
				else
				{
					// Incorrect password
					echo "Not found";
				}
			}
			else
			{
				// No users with specified data
				echo "Not found";
			}
		}
	}
	catch (Exception $ex)
	{
		echo "Not found";
	}

?>