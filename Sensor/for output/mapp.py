import serial

serial_port = 'COM3';
baud_rate = 9600; #In arduino, Serial.begin(baud_rate)
write_to_file_path = "output.txt";

ser = serial.Serial(serial_port, baud_rate)
required_bool = "2"
open(write_to_file_path, 'w').close()
while True:
    output_file = open(write_to_file_path, "a+");
    line = ser.readline()
    var2 = line.split(", ")
    url, no, count = line.partition(", ")
    line = line.decode("utf-8") #ser.readline returns a binary, convert to string
    url, no, bool = count.partition(", ")
    found_bool = bool.strip()
    if(found_bool is required_bool):
        output_file.write(url)
        output_file.write("\n")
        output_file.close()
        #break
    #output_file.write(line)
