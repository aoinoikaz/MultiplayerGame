<?php

	define('MYSQL_USER', 'id2476575_root');
	 
	//Our MySQL password.
	define('MYSQL_PASSWORD', '1234qwerasdfzxcv');
	
	//The server that MySQL is located on.
	define('MYSQL_HOST', 'localhost');
	
	//The name of our database.
	define('MYSQL_DATABASE', 'id2476575_dsn');
	
	// some PDO options & configuration details.
	// set the error mode to "Exceptions".
	// turn off emulated prepared statements
	$pdoOptions = array
	(
		PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
		PDO::ATTR_EMULATE_PREPARES => false
	);
	

	// connect to MySQL and instantiate the PDO object.
	$pdo = new PDO
	(
		'mysql:host=' . MYSQL_HOST . ';dbname=' . MYSQL_DATABASE, //DSN
		MYSQL_USER, 
		MYSQL_PASSWORD, 
		$pdoOptions 
	);


	// if values are empty
	if(!isset($_REQUEST['dsnloginusername']) || !isset($_REQUEST['dsnloginpassword']))
	{
		echo "EMPTYBOTTYBOI";
	}
	else
	{
		$username = trim($_REQUEST['dsnloginusername']);
		$password = trim($_REQUEST['dsnloginpassword']);
		
		// retrieve the user account information for the given username.
		$sql = "SELECT sUsername, sPassword FROM tAccounts WHERE sUsername = :Username AND sPassword = :Password";
		
		// hence the 'prepared statements' lool
		$stmt = $pdo->prepare($sql);
		
		// bind values
		$stmt->bindValue(':Username', $username);
		$stmt->bindValue(':Password', $password);
		
		// execute sql statement
		$stmt->execute();
		
		$pdo = null;
		
		// fetch row / data
		$user = $stmt->fetch(PDO::FETCH_ASSOC);
		
		if ($user === false) 
		{
			echo "DsnLoginOpFailed";
		}
		else
		{
			echo "DsnLoginOpSuccess";
		}
	}		
?>