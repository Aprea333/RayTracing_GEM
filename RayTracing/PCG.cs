namespace RayTracing;

public class PCG
{
    public ulong state = 0;
    public ulong inc = 0;

    public PCG(ulong init_state = 42, ulong init_seq = 54)
    {
        state = 0;
        inc = (init_seq << 1) | 1;
        random();
        state += init_state;
        random();
    }

    public uint random()
    {
        ulong oldstate = state;
        state = oldstate * 6364136223846793005ul + inc;
        uint xorshifted = (uint)(((oldstate >>> 18) ^ oldstate) >>> 27);
        uint rot = (uint)(oldstate >>> 59);
        return (xorshifted >>> (int)rot) | (xorshifted << (int)((-rot) & 31));
    }

    public float random_float()
    {
        return ((float) random()) / 0xffffffff;
    }
}