#GenericMapper

##What is GenericMapper?

It is a component that helps developers fix this problem brings us to map one object to another .
#Why use GenericMapper?
All developers agree that all we need do is apply a logic correctly, make good code and deliver a system with quality but as well we agree is that there are things that take us long but necessary , eg create DTOs (Data Transfer Objects) and to map an entity to a DTO is one of them .

It is simple to use and is very efficient.

#Using GenericMapper
We have a User entity with properties and one of those properties is another entity called Address

    public class User
    {
    	public string FirstName {get; set; }
    	public string LastName {get; set; }
    	public Address _Address {get; set; }
    }

    public class Address
    {
    	public string Street {get; set; }
    	public string Number { get; set; }
    	public string State {get; set; }
    }

And we are interested in a User Dto containing only first name and state .

    public class UserDto
    {
	    [ MapAttribute ( "FirstName")]
	    public string Name {get; set; }
	    
	    [ MapAttribute ( "_Address.State" ) ]
	    public string State {get; set; }
    }

With this mapping GenericMapper may already work, and is this way.

`var userDto = GenericMapper.MapObject <User, UserDto> ( user) ;`