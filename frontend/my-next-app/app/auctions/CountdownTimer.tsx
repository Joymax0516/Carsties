'use client';
import React from 'react';
import { CgPacman } from 'react-icons/cg';
import Countdown, { zeroPad } from 'react-countdown';

// Define Props interface
type Props = {
  auctionEnd: string; // Date or timestamp
}
// Define the renderer function with correct syntax
const renderer = ({
  days = 0, // Default value of 0 if days is undefined
  hours,
  minutes,
  seconds,
  completed,
}: {
  days?: number; // Optional number
  hours: number;
  minutes: number;
  seconds: number;
  completed: boolean;
}) => {
  return (
    <div className={`
      border-2
      border-white
      text-white py-1 px-2
      rounded-lg flex justify-center
      ${completed ?
        'bg-red-600' : (days === 0 && hours < 10)
        ? 'bg-amber-600':'bg-green-600'}
    `}>
        {completed ? (
          <span>Auction finished</span>
        ) : (
          <span suppressHydrationWarning>
            {zeroPad(days)}:{zeroPad(hours)}:{zeroPad(minutes)}:{zeroPad(seconds)}
          </span>
       )}
    </div>
  )
};

// Define the CountdownTimer component
export default function CountdownTimer({ auctionEnd }: Props) {
  return (
    <div>
      <Countdown date={auctionEnd} renderer={renderer} />
    </div>
  );
}
