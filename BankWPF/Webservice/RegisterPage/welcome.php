<?php
	session_start();
	
	if (!$_SESSION['registerdone'])
	{
		header('Location: index.php');
		exit();
	}
	
	// Delete session variables
	if(isset($_SESSION['fr_nick'])) unset($_SESSION['fr_nick']);
	if(isset($_SESSION['fr_password1'])) unset($_SESSION['fr_password1']);
	if(isset($_SESSION['fr_password2'])) unset($_SESSION['fr_password2']);
	
	// Delete registration errores
	if(isset($_SESSION['e_nick'])) unset($_SESSION['e_nick']);
	if(isset($_SESSION['e_password1'])) unset($_SESSION['e_password1']);
	if(isset($_SESSION['e_password2'])) unset($_SESSION['e_password2']);
	
?>

<!DOCTYPE HTML>
<html lang="pl">
<head>
<title>Rejestracja</title>
<meta charset="utf-8"/>
<link rel="stylesheet" href="css/style.css" type="text/css" />
</head>
<body>
<h2>Dziekujemy za rejestracje, mozesz teraz zalogowac sie w aplikacji.</h2>
</body>
</html>