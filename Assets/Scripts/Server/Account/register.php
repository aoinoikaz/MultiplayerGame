<?php

ini_set ("display_errors", "1");
error_reporting(E_ALL);

	// our mysql user
	define('MYSQL_USER', 'u454264411_devon');
	 
	//Our MySQL password.
	define('MYSQL_PASSWORD', 'guitar1234');
	 
	//The server that MySQL is located on.
	define('MYSQL_HOST', 'mysql.hostinger.co.uk');
	 
	//The name of our database.
	define('MYSQL_DATABASE', 'u454264411_zmoba');
 
 	// some PDO options & configuration details.
 	// set the error mode to "Exceptions".
    // turn off emulated prepared statements. idk why just do et
	$pdoOptions = array
	(
    	PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
    	PDO::ATTR_EMULATE_PREPARES => false
	);
 

 	// connect to MySQL and instantiate the PDO object.
 	$pdo = new PDO
	(
	    "mysql:host=" . MYSQL_HOST . ";dbname=" . MYSQL_DATABASE, //DSN
	    MYSQL_USER, //Username
	    MYSQL_PASSWORD, //Password
	    $pdoOptions //Options
	);
		
	// if variables aren't set
	if(!isset($_REQUEST['Username']) || !isset($_REQUEST['Password']) || !isset($_REQUEST['Email']))
	{
		echo "Empty";
		
	}
	// if values are set
	else
	{
		$username = trim($_REQUEST['Username']);
		$password = trim($_REQUEST['Password']);
		$email = trim($_REQUEST['Email']);
	
		// construct the SQL statement and prepare it.
		$sql = "SELECT * FROM accounts WHERE Username = :Username OR Email = :Email";
		$stmt = $pdo->prepare($sql);
		
		// bind the provided values to our prepared statement.
		$stmt->bindValue(':Username', $username);
		$stmt->bindValue(':Email', $email);
		
		// execute the query
		$stmt->execute();
		
		// fetch the result (row)
		$row = $stmt->fetch(PDO::FETCH_NUM);
		 
		//if the provided username already exists - display error.
		if($row[0] > 0)
		{
			echo "AlreadyExists";

		}
		else
		{
			//Prepare our INSERT statement.
			//insert into the account table, and prepare to bind values
			$sql = "INSERT INTO accounts (Username, Password, Email, GlobalLevel) VALUES (:Username, :Password, :Email, 1)";
			$stmt = $pdo->prepare($sql);
			
			//Bind our variables using the name of binding param on left and value on right
			$stmt->bindValue(':Username', $username);
			$stmt->bindValue(':Password', md5($password));
			$stmt->bindValue(':Email', $email);
			
			//Execute the statement and insert the new account.
			$result = $stmt->execute();
			
			//If the signup process is successful.
			if($result)
			{
				// unity will look for this echo
				echo "Success";
			}
			else 
			{
				echo "Failed";
			}
		}
	}
?>