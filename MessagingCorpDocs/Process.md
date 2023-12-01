
1. Client sends /registerMe?challenge="", where challenge is some arbit string which the user decides to put
2. Client gets back private key, and symmetric key stays on server (bc magic)
- Client send register if ok to use
- gets a one time token, then needs to make symmetric
- out of symettric gets asymmetric for chatrooms
- 2 ppl chat = 2 asymmetric keypairs, resolved with 2 symmetrics
	- 2 symmetrics has to be refreshed on x timespan
- if not set, after session the chat will be get destroyes