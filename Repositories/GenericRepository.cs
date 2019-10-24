using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Exceptions;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;
using PolyPort.Demo.Data;

namespace PolyPort.Demo.Repositories 
{  
  public abstract class GenericRepository<T> : IGenericRepository<T> where T : class  
  {  
    protected string _collection;
    
    protected IDemoContext _context;
  
    public GenericRepository(IDemoContext context, string collection)
    {
      this._context = context;
    }  
 
  
    public virtual async Task<ResultSet<dynamic>> GetAllAsync()  
    {
      return await this._context.SaveChanges("g.V()"); 
    } 
   
    public abstract Task<ResultSet<dynamic>> AddAsync(T t);
  
    // public virtual async Task<T> FindAsync(Expression<Func<T, bool>> match)  
    // {  
    //   return await _context.Set<T>().SingleOrDefaultAsync(match);  
    // } 
  
    // public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)  
    // {  
    //   return await _context.Set<T>().Where(match).ToListAsync();  
    // }
  
    // public virtual async Task<int> DeleteAsync(T entity)  
    // {  
    //   using (var transaction = _context.Database.BeginTransaction()) {
    //     try {
    //       _context.Set<T>().Remove(entity);  
    //       transaction.Commit();
    //       return await _context.SaveChangesAsync();
    //     } catch {
    //       transaction.Rollback();
    //       return -1;
    //     }
    //   }  
    // } 
  
    // public virtual async Task<T> UpdateAsync(T t, object key)  
    // { 
    //   using (var transaction = _context.Database.BeginTransaction()) {
    //     try {
    //       if (t == null)  
    //         return null;  
    //       T exist = await  _context.Set<T>().FindAsync(key);  
    //       if (exist != null)  
    //       {  
    //         _context.Entry(exist).CurrentValues.SetValues(t);  
    //         await _context.SaveChangesAsync(); 
    //         transaction.Commit(); 
    //       }  
    //       return exist;
    //     } catch {
    //       transaction.Rollback();
    //       return null;
    //     }
    //   }  
    // } 
  
    // public async Task<int> CountAsync()  
    // {  
    //   return await _context.Set<T>().CountAsync();  
    // }  
  
    // public async virtual Task<int> SaveAsync()  
    // {  
    //   return await _context.SaveChangesAsync();  
    // } 
  
    // public virtual async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate)  
    // {  
    //   return await _context.Set<T>().Where(predicate).ToListAsync();  
    // }  
  
    private bool disposed = false;  
    protected virtual void Dispose(bool disposing)  
    {  
      if (!this.disposed)  
      {  
        if (disposing)
        {  
          this._context.Dispose();  
        }  
        this.disposed = true;  
      }  
    }  
  
    public void Dispose()  
    {  
      Dispose(true);  
      GC.SuppressFinalize(this);  
    }

    }  
} 