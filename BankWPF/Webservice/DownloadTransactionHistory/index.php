<?php

	// Include sql-server data
	require_once "connect.php";
	
	// Get account data by POST method from C# App
	$idaccount = $_POST['id'];
	
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
			$result = $connection->query("SELECT * FROM transactions WHERE idaccount = '$idaccount' ORDER BY date DESC ");
			// If failed to send query, throw exception
			if (!$result) throw new Exception($connection->error);
			
			// Get the amount of founded transactions
			$amount = mysqli_num_rows($result);
            
			// Output this amount
            echo "Found: ".$amount."|";
			
			// Output every transaction
			for ($i = 1; $i <= $amount; $i++) 
			{
				$row = mysqli_fetch_assoc($result);
				echo $row['paymentway']."/".$row['value']."/".$row['message']."/".$row['date']."|";
			}
		}
	}
	catch (Exception $ex)
	{
		echo "Found: 0";
	}

?>