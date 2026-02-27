import json

def try_deserialize(message):
    try:
        data = json.loads(message)
        return data
    except:
        print("Niepoprawny JSON")
        return