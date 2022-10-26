print("I hate python")
#This is a basic declaration of a python class
class Message:
    #This is a basic declaration of a public python variable with a value
    message = "Hello world"
    #Declaration of an "empty" variable|| Think of null
    empty_message = None
    #Delaration of a protected variable
    _message = "This is a protected message"
    #Delaration of a private variable
    __message = "This is a private message"
    #This is a basic declaration of a python method
    def SendMessage():
        print("This is not hello world")
    
    #def SendProtectedMessage():
    #   print(this._message)
    #def SendPrivateMessage():
    #    print(this.__message)

myMessage = Message
myMessage.SendMessage
