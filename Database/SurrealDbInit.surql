-- Create Namespaces
DEFINE NAMESPACE MessagingCorpGeneral;
DEFINE NAMESPACE MessagingCorpVault;

-- Create Databases in Namespaces
USE NS MessagingCorpGeneral;
DEFINE DATABASE users;

DEFINE NAMESPACE MessagingCorpVault;
DEFINE DATABASE keys;

-- Create Users for access
USE NS MessagingCorpGeneral;
DEFINE USER corpadmin ON ROOT PASSWORD 'lulz123' ROLES OWNER;

USE NS MessagingCorpGeneral DB users;

BEGIN TRANSACTION;
USE NS MessagingCorpGeneral DB users;
-- Create the admin user with specific numeric id
CREATE ONLY users:admin SET UserId = 0, UserName = 'Administrator',Password = 'Lol123';

COMMIT TRANSACTION;