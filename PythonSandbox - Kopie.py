class TextBase:
    msg : str
    def __init__(self, input_msg: str) -> None:
        self.msg = input_msg
    def ChangeText(self, input_msg: str) -> None:
        self.msg = input_msg
    def ToString(self) -> str:
        return self.msg

text_msg2 = TextBase("Lol")
text_msg = "I love Python"
print(text_msg)

def ChangeText(old_msg):
    old_msg = "I hate Python"
    return old_msg

text_msg = ChangeText(text_msg)



print(text_msg)

print(text_msg2.ToString())
text_msg2.ChangeText("Me too XD")
print(text_msg2.ToString())
