import jcifs.smb.*;
import java.io.BufferedReader;
import java.io.InputStreamReader;

class PingTest{
    public static void main(String[] args) {
        try {
            String user = "candicedhuri@gmail.com:candice1234";
            NtlmPasswordAuthentication auth = new NtlmPasswordAuthentication(user);
            String path = "smb://desktop-atov87t/test.txt";
            SmbFile sFile = new SmbFile(path, auth);
            BufferedReader reader = new BufferedReader(new InputStreamReader(new SmbFileInputStream(sFile)));
            String line = reader.readLine();
            while (line != null) {
                line = reader.readLine();
                System.out.println(line);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}