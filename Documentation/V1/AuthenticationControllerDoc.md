## Register Api
### Summery 
	Register a new user in the system and assign this user to the default role that is **User** role
### Parameters
	take a **RegistrationRequestDto** that contains the user information such as : 
	FirstName , LastName , Email , PhoneNumber and Password
### Returns
	returns the necessary information for the registered user


## Add New User Role Api
### Summery 
	Add new User Role to the system 
### Parameters 
	take one string parameter which is the role name
### Returns
	returns AddNewRoleResponseDto that contains information of the adding result

## Login Api
### Summery 
	try to login the user and generate JWT for the logged in user
### Parameters 
	take one parameter of type LoginRequestDto which contains the email and password 
### Returns 
	LoginRequestDto which contains the important user information and the generated token

## Assign User To Role
### Summery
	try to assign the user to the specified role name
### parameters
	take one parameter AssignUserToRoleRequestDto which contains the User Id and the role name
### Returns
	returns AssignUserToRoleResponseDto that determine if the user is assigned or not