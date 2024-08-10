namespace HangFire3.IService
{
    public interface IReprocess
    {

        void CallStoreProcedureFirst();
        void CallSecondStoreProcedure();
        void CallThirdStoreProcedure();
        void CallFourthStoreProcedure();
    }
}
