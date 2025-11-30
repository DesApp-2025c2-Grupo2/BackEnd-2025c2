namespace Domain.DataModels;

public class ReportDataList<T> : List<T> where T : ReportDataRow { }
public abstract class ReportDataRow { }
