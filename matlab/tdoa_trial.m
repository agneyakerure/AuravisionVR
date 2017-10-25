% 2 microphone TDOA using Cross Power Spectrum
% http://www.academia.edu/5280022/Direction_of_arrival_estimation_by_cross-power_spectrum_phase_analysis_using_prior_distributions_and_voice_activity_detection_information
function [sample_delay, theta_deg]=tdoa_trial(sig, chunk_size, Fs, dist)

[samples, channels] = size(sig);
chunks = ceil(samples/chunk_size) -1;

sample_delay = zeros(chunks, 1);
theta_deg = zeros(chunks, 1);

for i=0:chunks-1
    
    chunk_i = sig( (i*chunk_size)+1:(i+1)*chunk_size, 1); %channel 1
    chunk_j = sig( (i*chunk_size)+1:(i+1)*chunk_size, 2); %channel 2
    numerator = fft(chunk_i) .* conj(fft(chunk_j));
    denominator = abs(fft(chunk_i)) .* abs(fft(chunk_j));
    csp = ifft(numerator./ denominator);
    [y, idx] = max(csp);
    theta = abs(asind( (idx * v) / (dist * Fs) ));
    sample_delay(i+1, 1) = idx;
    theta_deg(i+1, 1) = theta; 
    
end