<?php

	// Start user's session
	session_start();
	
?>

<!DOCTYPE HTML>
<html lang="pl">
<head>
<title>Rejestracja</title>
<script src='https://www.google.com/recaptcha/api.js'></script>
<link rel="stylesheet" href="css/style.css" type="text/css" />
</style>
<meta charset="utf-8"/>
</head>
<body> 
<h1>Bank</h1>
<h2>Rejestracja</h2>
<div id="container">
<form method="post" action="register.php">
	<input type="text" value="<?php
	if (isset($_SESSION['fr_nick']))
	{
		echo $_SESSION['fr_nick'];
		unset($_SESSION['fr_nick']);
	}
	?>" name="nick" placeholder="Nick" onfocus="this.placeholder=''" onblur="this.placeholder='Nick'"/> <br />
	
	<?php
		if (isset($_SESSION['e_nick']))
		{
			echo '<div class="error">'.$_SESSION['e_nick'].'</div>';
			unset($_SESSION['e_nick']);
		}
	?>
	
	<input type="password" value="<?php
	if (isset($_SESSION['fr_password1']))
	{
		echo $_SESSION['fr_password1'];
		unset($_SESSION['fr_password1']);
	}
	?>" name="password1" placeholder="Haslo" onfocus="this.placeholder=''" onblur="this.placeholder='Haslo'"/> <br />
	
	<?php
		if (isset($_SESSION['e_password']))
		{
			echo '<div class="error">'.$_SESSION['e_password'].'</div>';
			unset($_SESSION['e_password']);
		}
	?>
	
	<input type="password" value="<?php
	if (isset($_SESSION['fr_password2']))
	{
		echo $_SESSION['fr_password2'];
		unset($_SESSION['fr_password2']);
	}
	?>" name="password2" placeholder="Powtorz haslo" onfocus="this.placeholder=''" onblur="this.placeholder='Powtorz haslo'"/>

	<br />
	<br />
	
	<div class="g-recaptcha" data-sitekey="6Le0-CMUAAAAAMDjiZmFzUaBYERUdzdKogC-QMlm"></div> <br />
	<?php
		if (isset($_SESSION['e_bot']))
		{
			echo '<div class="error">'.$_SESSION['e_bot'].'</div>';
			unset($_SESSION['e_bot']);
		}
	?>
	<input type="submit" value="Zarejestruj sie" />
</form>
</div>
</body>
</html>