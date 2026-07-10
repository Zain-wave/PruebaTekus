public class Provider{
    public int Id{get; private set;}

    public string Nit{get; private set;}
    public string Name{get; private set;}
    public string Website {get; private set;}
    public string Email{get; private set;}

    public ICollection<Service> Services{get; private set;}
    }
// {   }