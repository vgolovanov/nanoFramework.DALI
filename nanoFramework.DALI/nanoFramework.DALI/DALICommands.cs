using System;

namespace nanoFramework.DALI
{
    public static class DALICommands
    {
        public const byte DALI_CMD_OFF = 0x00;
        public const byte DALI_CMD_UP = 0x01;
        public const byte DALI_CMD_DOWN = 0x02;
        public const byte DALI_CMD_STEP_UP = 0x03;
        public const byte DALI_CMD_STEP_DOWN = 0x04;
        public const byte DALI_CMD_MAX_LEVEL = 0x05;
        public const byte DALI_CMD_MIN_LEVEL = 0x06;
        public const byte DALI_CMD_STEP_DOWN_AND_OFF = 0x07;
        public const byte DALI_CMD_ON_AND_STEP_UP = 0x08;
        public const byte DALI_CMD_ENABLE_DAPC_SEQUENCE = 0x09;
        public const byte DALI_CMD_GO_TO_SCENE = 0x10;//possible values from 0x10 to 0x1F

        /*
        * ->DALI configuration commands (32 ... 128)
        * (IEC-60929, E.4.3.3.2)
        */
        public const byte DALI_CMD_RESET = 32;
        public const byte DALI_CMD_STORE_LEVEL = 33;
        public const byte DALI_CMD_STORE_DTR_SA = 128;

        /*
        * ->DALI query commands (144 ... 255)
        * (IEC-60929, E.4.3.3.3)
        */
        public const byte DALI_CMD_QUERY_STATUS = 144;
        public const byte DALI_CMD_QUERY_BALLAST = 145;
        public const byte DALI_CMD_QUERY_FAIL = 146;
        public const byte DALI_CMD_QUERY_MISS_SA = 150;
        public const byte DALI_CMD_QUERY_DTR = 152;
        public const byte DALI_CMD_QUERY_PHY_MIN = 154;
        public const byte DALI_CMD_QUERY_LEVEL = 160;
        public const byte DALI_CMD_QUERY_MAX = 161;
        public const byte DALI_CMD_QUERY_MIN = 162;

        /*
        * ->DALI special commands (144 ... 255)
        * (IEC-60929, E.4.3.3.4)
        * note that the following codes
        * does not match with the commands
        * numbers in the paragraph E.4.3.3.4.
        * These code are the real command
        * representation from binary code.
        * e.g.:
        * Command 261: 1010 1001 0000 0000 "COMPARE"
        * 1010 1011 = 169
        */
        public const byte DALI_CMD_TERMINATE = 161;
        public const byte DALI_CMD_STORE_DTR = 163;
        public const byte DALI_CMD_INITIALISE = 165;
        public const byte DALI_CMD_RANDOMISE = 167;
        public const byte DALI_CMD_COMPARE = 169;
        public const byte DALI_CMD_WITHDRAW = 171;

        public const byte DALI_CMD_SEARCHADDRH = 177;
        public const byte DALI_CMD_SEARCHADDRM = 179;
        public const byte DALI_CMD_SEARCHADDRL = 181;

        public const byte DALI_CMD_PROG_SA = 183;
        public const byte DALI_CMD_VERIFY_SA = 185;
        public const byte DALI_CMD_QUERY_SA = 187;
    }
}
