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
DEFINE USER corpadmin ON NAMESPACE PASSWORD 'lulz123' ROLES OWNER;

-- Create the admin user with specific numeric id
CREATE users:admin CONTENT {
	UserId: 0, 
	UserName: 'Administrator',
	Password: 'Lol123', 
	ActiveLobbyParticipations: ['0'],
};
