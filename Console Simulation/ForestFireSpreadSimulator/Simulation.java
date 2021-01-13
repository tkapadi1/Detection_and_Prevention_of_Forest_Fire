import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Timer;
import java.util.TimerTask;
import java.util.Random;

import java.io.BufferedReader;
import java.io.InputStreamReader;

import jcifs.smb.*;

class Simulation extends TimerTask {
    
    // User Constants
    public static final int FIRE_SPREAD = 1000;
    public static final int SPEED = 80;
    public static final int SENSORDENSITY = 4;
    public static final int GRID = 52;
    public static final int L_PADDING_OFFSET = 45;
    public static final int T_PADDING = 2;
    public static final int ALGORITHM = 1;
    public static final int SEEDI = 33;
    public static final int SEEDJ = 33;
     
    // ANSI Prefabs
    public static final String ANSI_RESET = "\u001B[0m";
    public static final String ANSI_RED = "\u001B[31m";
    public static final String ANSI_GREEN = "\u001B[32m";
    public static final String ANSI_BLUE = "\u001B[35m";
    public static final String ANSI_YELLOW = "\u001B[33m";
    public static final String ANSI_CYAN = "\u001B[36m";
    public static final String CLEAR = "\033[H\033[2J";

    // Constants
    public static final String FIRE = ANSI_YELLOW + "F" + ANSI_RESET;
    public static final String ACT_SENSOR = ANSI_RED + "S" + ANSI_RESET;
    public static final String WARNED_SENSOR = ANSI_GREEN + "S" + ANSI_RESET;
    public static final String GRASS = " ";
    public static final String SENSOR = "S";
    public static final String L_PADDING = String.format("%"+ L_PADDING_OFFSET +"s", " ");

    // Counters
    static int fireSpread = 0;
    static int sensorsWarned = 0;
    static int sensorsTriggered = 0;

    // Synchronized
    static boolean lock = true;
    static Simulation dummy;

    // Graphs 
    static String map[][] = new String[GRID][GRID];
    static int sensormap[][] = new int[GRID][GRID];

    // Lists
    static ArrayList<Posi> list = new ArrayList<>();
    static ArrayList<Posi> sensorList = new ArrayList<>();

    static Random rand = new Random();

    public static void main(String args[]) {

        
        dummy = new Simulation();
        Posi seedPosi = new Posi(SEEDI,SEEDJ);
        list.add(seedPosi);
        resetMap();
        TimerTask timerTask = new Simulation();
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
        // Running simulation task as a daemon thread
        Timer timer = new Timer(true);
        timer.scheduleAtFixedRate(timerTask, 0, SPEED);
        System.out.println("TimerTask started");

        synchronized (dummy) {
            try {
                dummy.wait();
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }

    @Override
    public void run() {
        clearScreen();
        spreadFire();
        displayMap();
        predictFire();
        displayStats();
        fireSpread++;
        if(fireSpread > FIRE_SPREAD){
            synchronized(dummy){
                dummy.notify();
            }
        }
    }

    /**
     * Displays stats 
     */
    public static void displayStats(){
        System.out.println("\n" + L_PADDING + "Blocks affected by fire - " + ANSI_GREEN + (fireSpread + 1) + ANSI_RESET);
        System.out.println(L_PADDING + "Sensors Triggered - " + ANSI_GREEN + sensorsTriggered + ANSI_RESET);
        System.out.println(L_PADDING + "Sensors Warned - " + ANSI_GREEN + sensorsWarned + ANSI_RESET);
    }

    /**
     * Displays the map grid
     */
    public static void displayMap(){
        String upperBorder = "";
        int dGRID = GRID*2+1;
        for(int i = 0; i <= dGRID; i++, upperBorder+="_");
        System.out.print(L_PADDING + upperBorder + "\n");
        for(int i = 0; i < GRID; i++){
            System.out.print(L_PADDING + "|");
            for(int j = 0; j < GRID; j++){
                System.out.print(" " + map[i][j]);
                if(j == (GRID-1)){
                    System.out.print("|");
                }
            }
            System.out.println("");
        }
        System.out.print(L_PADDING + upperBorder + "\n");
    }

    /**
     * Detects and predicts next fire
     */
    public static void predictFire(){
        switch(ALGORITHM){
            case 1:
                for(Posi s : sensorList){
                    for(int k = -SENSORDENSITY; k <= SENSORDENSITY; k+=SENSORDENSITY){
                        for(int l = -SENSORDENSITY; l <= SENSORDENSITY; l+=SENSORDENSITY){
                            if(s.getI() < SENSORDENSITY || s.getI() >= (GRID-SENSORDENSITY) || s.getJ() < SENSORDENSITY || s.getJ() >= (GRID-SENSORDENSITY)){
                                continue;
                            }
                            if(map[s.getI()+k][s.getJ()+l].equals(SENSOR)){
                                map[s.getI()+k][s.getJ()+l] = WARNED_SENSOR;
                                sensorsWarned++;
                            }
                        }
                    }
                }
                break;
                case 2:
                Posi p = list.get(list.size() - 1);
                int i = p.getI();
                int j = p.getJ();
                if(i % SENSORDENSITY == 0){
                    int sensorPassedLeft = (j / SENSORDENSITY) * SENSORDENSITY;
                    int sensorPassedRight = sensorPassedLeft + SENSORDENSITY;
                    if(map[i][sensorPassedLeft].equals(SENSOR)){
                        map[i][sensorPassedLeft] = WARNED_SENSOR;
                        sensorsWarned++;
                    }
                    if(map[i][sensorPassedRight].equals(SENSOR)){
                        map[i][sensorPassedRight] = WARNED_SENSOR;
                        sensorsWarned++;
                    }
                }else if(j % SENSORDENSITY == 0){
                    int sensorPassedUp = (i / SENSORDENSITY) * SENSORDENSITY;
                    int sensorPassedDown = sensorPassedUp + SENSORDENSITY;
                    if(map[sensorPassedUp][j].equals(SENSOR)){
                        map[sensorPassedUp][j] = WARNED_SENSOR;
                        sensorsWarned++;
                    }
                    if(map[sensorPassedDown][j].equals(SENSOR)){
                        map[sensorPassedDown][j] = WARNED_SENSOR;
                        sensorsWarned++;
                    }
                }
                break;
        }
    }

    /**
     * Spreads fire particles randomly using randomize grid
     */
    public static void spreadFire(){
        
        while(true){
            int listIndex = rand.nextInt(list.size());
            int option = rand.nextInt(8) + 1;
            Posi posi = list.get(listIndex);

            int i = posi.getI();
            int j = posi.getJ();

            switch(option){
                case 1:
                    i+=1;
                    j+=1;
                    break;
                case 2:
                    i+=1;
                    break;
                case 3:
                    i+=1;
                    j-=1;
                    break;
                case 4:
                    j-=1;
                    break;
                case 5:
                    i-=1;
                    j-=1;
                    break;
                case 6:
                    i-=1;
                    break;
                case 7:
                    i-=1;
                    j+=1;
                    break;
                case 8:
                    j+=1;
                    break;
            }

            if(i < 0 || i > (GRID-1) || j < 0 || j > (GRID-1)){
                continue;
            }
            else if(map[i][j].contains(GRASS)){
                map[i][j] = FIRE;
                for(int k = -1; k <= 1; k++){
                    for(int l = -1; l <= 1; l++){
                        if(i < 1 || i >= (GRID-1) || j < 1 || j >= (GRID-1)){
                            continue;
                        }
                        if(map[i+k][j+l].equals(SENSOR) || map[i+k][j+l].equals(WARNED_SENSOR)){
                            map[i+k][j+l] = ACT_SENSOR;
                            if(sensormap[i+k][j+l] == 0){
                                sensormap[i+k][j+l] = 1;
                                sensorList.add(new Posi(i+k, j+l));
                                sensorsTriggered++;
                            }
                        }
                    }
                }
                list.add(new Posi(i,j));
                break;
            }

        }
        
    }

    /**
     * Clears the screen based on the platform being used
     */
    public static void clearScreen(){
        try {
            final String os = System.getProperty("os.name");

            if (os.contains("Windows"))
                new ProcessBuilder("cmd", "/c", "cls").inheritIO().start().waitFor();
            else{
                System.out.print(CLEAR);  
                System.out.flush();
            }
            for(int i = 0 ; i < T_PADDING; i++){
                System.out.println("");
            }
        } catch (Exception ex) {
            ex.printStackTrace();
        } 
    }

    public static void resetMap(){
        for(int i = 0; i < GRID; i++){
            for(int j = 0; j < GRID; j++){
                if(i != 0 && j != 0 && i % SENSORDENSITY == 0 && j % SENSORDENSITY == 0){
                    map[i][j] = SENSOR;
                }else{
                    map[i][j] = GRASS;
                }
            }
        }
    } 

}

/**
 * Data Class used to denote position
 */
class Posi{

    int i, j;
    public Posi(int i, int j){
        this.i = i;
        this.j = j;
    }
    public int getI() {
        return i;
    }
    public int getJ() {
        return j;
    }

}