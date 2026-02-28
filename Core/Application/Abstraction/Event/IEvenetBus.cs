using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstraction.Event
{
    public interface IEvenetBus
    {
        Task PublishAsync<T>(T Message, CancellationToken cancellationToken = default) where T : class;
    }
}
