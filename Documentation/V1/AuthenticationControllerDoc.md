## Register Endpoint
### Summary 
	Register a new user in the system and assign this user to the default role that is **User** role
### Parameters
	Take a **RegistrationRequestDto** that contains the user information such as : 
	FirstName , LastName , Email , PhoneNumber and Password
### Returns
	Returns the necessary information for the registered user

## Add New User Role Endpoint
### Summary 
	Add new User Role to the system 
### Parameters 
	Take one string parameter which is the role name
### Returns
	Returns Add New Role Response Dto that contains information of the adding result

## Login Endpoint
### Summary 
	Try to login the user and generate JWT for the logged in user
### Parameters 
	Take one parameter of type LoginRequestDto which contains the email and password 
### Returns 
	Returns Login Request Dto which contains the important user information and the generated token

## Assign User To Role Endpoint
### Summary
	Try to assign the user to the specified role name
### parameters
	Take one parameter AssignUserToRoleRequestDto which contains the User Id and the role name
### Returns
	Returns Assign User To Role Response Dto that determine if the user is assigned or not