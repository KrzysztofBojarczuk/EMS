﻿using EMS.CORE.Entities;
using EMS.CORE.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Features.Address.Commands
{
    public record AddAddressCommand(AddressEntity address) : IRequest<AddressEntity>;

    public class AddAddressCommandHandler(IAddressRepository addressRepository, IPublisher mediator)
        : IRequestHandler<AddAddressCommand, AddressEntity>
    {
        public async Task<AddressEntity> Handle(AddAddressCommand request, CancellationToken cancellationToken)
        {
            var address = await addressRepository.AddAddressAsync(request.address);
            return address;
        }
    }
}
