import Image from 'next/image'
import React from 'react';
import CountdownTimer from './CountdownTimer';
import CarImage from './CarImage';
import { Auction } from '@/types';
type Props = {
  auction: Auction;

}
export default function AuctionCard({auction}:Props) {
   return (
    <a href="#" className='group'>
      <div className="w-full bg-gray-200 aspect-w-16 aspect-h-10 rounded-lg overflow-hidden">
         <CarImage imageUrl={auction.imageUrl} />
        <div className='absolute bottom-2 left-2'>
          <CountdownTimer auctionEnd={auction.auctionEnd}/>
        </div>
      </div>
      <div className='flex justify-between items-center mt-4'>
          <p className='text-gray-700'>{auction.make} {auction.model}</p>
          <p className='font-semibold text-sm'>{auction.year}</p>
      </div>
    </a>
   )
}