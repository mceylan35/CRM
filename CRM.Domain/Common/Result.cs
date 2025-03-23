using System;
using System.Collections.Generic;

namespace CRM.Domain.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public bool IsFailure => !IsSuccess;

        public Result(bool isSuccess, string error)
        {
            if (isSuccess && !string.IsNullOrWhiteSpace(error))
                throw new InvalidOperationException("Başarılı bir sonuç hata içeremez");
            
            if (!isSuccess && string.IsNullOrWhiteSpace(error))
                throw new InvalidOperationException("Başarısız bir sonuç hata içermelidir");

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new Result(true, string.Empty);
        public static Result Failure(string error) => new Result(false, error);
        
        public static Result<T> Success<T>(T value) => new Result<T>(value, true, string.Empty);
        public static Result<T> Failure<T>(string error) => new Result<T>(default, false, error);
    }

    public class Result<T> : Result
    {
        private readonly T _value;
        
        public T Value 
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException("Başarısız bir sonuçtan değer alınamaz");
                
                return _value;
            }
        }

        public Result(T value, bool isSuccess, string error) 
            : base(isSuccess, error)
        {
            _value = value;
        }
    }
} 