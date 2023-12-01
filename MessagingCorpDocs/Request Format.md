## General
Endpoints:
- Actions: `http://127.0.0.1:8009/genposter`
- 
### Definitions
"FullEnc" = B64(Asym(B64(Sym(Data))))
"CorpEnc" = B64(CorpSym(Data))
"SymEnc" = B64(Sym(Data))
"AsymEnc" = B64(Asym(Data)) 
"SymKeySet" = Key + IV + (salt)
"AsymPriv" = Asym PrivKey
"AsymPub" = Asymc PubKey
"AsymKeySet" = AsymPub + AsymPriv
"FullEncSet" = SymKeySet + AsymKeySet

### Additional
Seperator for additional data : ";" (maybe make it confgurable)

## Actions
### Register User

-  The user needs the standard corp AES key in order to encrypt the register message (maybe in the future)
- If "ProvideSym" then generate keys etc for the user and send back
	- If not then key gets provided by user
- If "ProvideAsym"  then generate keys etc for the user and send back
	- if not then key gets provided by user

Request:
```

Body:
UserId == GrownCorp-User-04fd366c81fd;
Challenge == Challenge;
Action == register;
AdditionalData == Base64ed(StdCorpAes(Data)) + Base64ed(StdCorpAes(Challenge:::SomeMessageCorpConstant:::Challenge=))

mit Data=
Username;Password;SymmetricChoice;(SymmetricKey | "ProvideSym");AsymmetricChoice;(AsymmetricPublicKey | "ProvideAsym");
```
Response:
```
Vars:
keys = provided ? FullEncSet : nothing 
data = provided ? FullEnc(Data) : CorpEnc(Data)

Body:
B64(UID ; keys ; data)
```
- **Login User**
	- 