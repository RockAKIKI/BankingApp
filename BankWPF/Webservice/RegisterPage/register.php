<?php

	// Start user's session
	session_start();
	
	// Set the flag to true, if any error will occur, it will be false and pretends from register
	$everything_OK = true;
	$_SESSION['registerdone'] = false;
	
	////////////////////////////////////////////////////
	// Catch nickname from user's input
	$nick = $_POST['nick'];
	
	// Check nickname's length
	if ((strlen($nick) < 3) || (strlen($nick) > 20))
	{
		$everything_OK = false;
		$_SESSION['e_nick'] = "Nick musi posiadac od 3 do 20 znakow";
	}
	
	// Check nickname's characters
	if (ctype_alnum($nick) == false)
	{
		$everything_OK = false;
		$_SESSION['e_nick'] = "Nick moze skladac sie tylko z liter i cyfr (bez polskich znakow)";
	}
	
	////////////////////////////////////////////////////
	// Catch password from user's input
	$password1 = $_POST['password1'];
	$password2 = $_POST['password2'];
	
	// Check password's length
	if (strlen($password1) < 6 || strlen($password1) > 20)
	{
		$everything_OK = false;
		$_SESSION['e_password'] = "Haslo musi posiadac od 6 do 20 znakow";
	}
	
	// Check if first password equals second
	if ($password1 != $password2)
	{
		$everything_OK = false;
		$_SESSION['e_password'] = "Podane hasla nie sa identyczne";
	}
	
	// Hash password to make it unreadable by human
	$password_hash = password_hash($password1, PASSWORD_DEFAULT);
	/*
	// Bot or not? CAPTCHA
	$secretKey = "6Le0-CMUAAAAAN4jpDyCclv2bkJ0dW9j_rebrwGq";
	$checkCaptcha = file_get_contents('https://www.google.com/recaptcha/api/siteverify?secret='.$secretKey.'&response='.$_POST['g-recaptcha-response']);
	$answerCaptcha = json_decode($checkCaptcha);
	
	// Check if captcha was answered properly
	if (!$answerCaptcha->success)
	{
		$everything_OK = false;
		$_SESSION['e_bot'] = "Potwierdz ze nie jestes botem!";
	}
	*/
	/////////////////////////////////////////////
	// Remember user's inputs
	$_SESSION['fr_nick'] = $nick;
	$_SESSION['fr_password1'] = $password1;
	$_SESSION['fr_password2'] = $password2;
	
	// Include connection data
	require_once "connect.php";
	// Ban MySQL from reporting errors
	mysqli_report(MYSQLI_REPORT_STRICT);
		
	try
	{
		// Connect to database
		$connection = new mysqli($host, $db_user, $db_password, $db_name);
		// If failed, throw exception
		if ($connection->connect_errno != 0)
			throw new Exception(mysqli_connect_errno());
		// If succeded, register user
		else
		{
			// Check if user already exists
			$result = $connection->query("SELECT id FROM users WHERE login = '$nick'");
			if (!$result) throw new Exception($connection->error);
			$howManyUsers = $result->num_rows;
			if ($howManyUsers > 0)
			{
				$everything_OK = false;
				$_SESSION['e_nick'] = "Juz istnieje konto o takim nicku.";
			}
			
			////////////////////////////////////////////////
			// Everything is checked, lets do this!
			if ($everything_OK)
			{
				// Try to request query from database
				if ($connection->query("INSERT INTO users VALUES (NULL, '$nick', '$password_hash', 0)"))
				{			
					$_SESSION['registerdone'] = true;
					header('Location: welcome.php');
				}	
				else
					throw new Exception($connection->error);
				// Close our connection with database
				$connection->close();
			}
			else
			{
				// If something went wrong, send user to default page
				header('Location: index.php');
				// Close our connection with database
				$connection->close();
			}
			
			
		}
	}
	catch(Exception $e)
	{
		// If something went wrong
		header('Location: index.php');
	}

				
?>